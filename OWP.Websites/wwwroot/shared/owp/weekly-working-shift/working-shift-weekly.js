var workingShiftWeekly = (function ($) {
  var isHost = $(".body-content").attr("data-isHostWeekly") === "True";
  var isIFrame = $(".body-content").attr("data-isIFrame") === "True";
  var isHostShiftColor =
    $(".body-content").attr("data-isHostShiftColor") === "True";
  var isHostWeeklySection =
    $(".body-content").attr("data-isHostWeeklySection") === "True";
  var workingShiftSetting;
  var workingShiftUsers;
  var grid;
  var isFutureWeek;

  function findDefaultShiftIds(userId) {
    for (var i = 0; i < workingShiftUsers.length; i++) {
      if (workingShiftUsers[i].UserId === userId) {
        return workingShiftUsers[i].DefaultShiftIds;
      }
    }
  }

  function updateDefaultShiftIds(userId, defaultShiftIds) {
    for (var i = 0; i < workingShiftUsers.length; i++) {
      if (workingShiftUsers[i].UserId === userId) {
        workingShiftUsers[i].DefaultShiftIds = defaultShiftIds;
      }
    }
  }

  function parseInteger(value) {
    var x = parseFloat(value);
    if (!isNaN(value) && (x | 0) === x) {
      return x;
    }

    return undefined;
  }

  function isTime(value) {
    if (value.indexOf(" ") !== -1) {
      return false;
    }

    var words = value.split(":");
    if (words.length > 2) {
      return false;
    }

    var hour = parseInteger(words[0]);
    if (hour >= 0 && hour <= 23) {
      if (words.length === 2) {
        var minuteText = words[1].trim();
        if (minuteText === "") {
          return true;
        }

        var minute = parseInteger(minuteText);
        if (minute >= 0 && minute <= 59) {
          return true;
        }
      } else {
        return true;
      }
    }

    return false;
  }

  function filterAndOrderShifts(shifts, shiftSearchText) {
    if (shiftSearchText.length > 0) {
      shifts = shifts.filter(function (s) {
        return s.ShiftCode.indexOf(shiftSearchText) !== -1;
      });
      if (isTime(shiftSearchText)) {
        function getPoint(shift) {
          return (
            (shift.IsTimeShift ? 2 : 0) +
            (shift.ShiftCode.startsWith(shiftSearchText) ? 1 : 0)
          );
        }

        shifts = shifts.sort(function (s1, s2) {
          var point1 = getPoint(s1);
          var point2 = getPoint(s2);
          if (point1 === point2) {
            if (s1.ShiftCode > s2.ShiftCode) {
              return 1;
            } else if (s1.ShiftCode === s2.ShiftCode) {
              return -1;
            } else {
              return 0;
            }
          } else if (point1 > point2) {
            return -1;
          } else {
            return 1;
          }
        });
      }
    }

    return shifts;
  }

  function canEditable(userId, sectionId) {
    if (isIFrame) return false;
    if (!isHost) return false;
    if (!isHostWeeklySection) {
      return workingShiftData.currentSectionId === sectionId.toString();
    }

    return true;
  }

  function isShowOrderModeButton() {
    if (isHost) {
      if (
        workingShiftData.currentSectionId === "" &&
        isHostWeeklySection === false
      ) {
        return false;
      }

      return true;
    }

    return false;
  }

  function encodeShiftCode(shiftCode) {
    return $("<div />").text(shiftCode).html();
  }

  function findShiftById(shiftId) {
    if (shiftId === 0) {
      return undefined;
    }

    for (var shift of workingShiftSetting.shifts) {
      if (shift.ShiftId === shiftId) {
        return shift;
      }
    }
  }

  function removeWorkingShiftDataRow(userId) {
    var user;
    for (let i = 0; i < grid.data.length; i++) {
      user = grid.data[i];
      if (user.UserId === userId) break;
    }
    if (user === undefined) return;
    for (let j = 0; j < user.WorkingShifts.length - 1; j++) {
      user.WorkingShifts[j] = null;
    }
  }

  function updateWorkingShiftDataRow(userId, workingShifts, isPublished) {
    var user;
    for (var i = 0; i < grid.data.length; i++) {
      user = grid.data[i];
      if (user.UserId === userId) break;
    }
    if (user === undefined) return;
    if (isPublished) {
      workingShifts.splice(0, 0, user.WorkingShifts[0]);
    } else {
      workingShifts[7] = user.WorkingShifts[7];
    }
    user.WorkingShifts = workingShifts;
  }

  function queryHistoryLog(numberOfActions, objectId) {
    var queryData = {
      PageSize: numberOfActions,
      ObjectId: objectId,
    };
    var modalBody = $("#js-modalViewLog .modal-body .js-logTable");
    var oldTable = modalBody.find(".log-table");
    var newTableElement = '<table class="log-table"></table>';
    if (oldTable.length > 0) {
      oldTable.remove();
      modalBody.append(newTableElement);
    } else {
      modalBody.append(newTableElement);
    }
    $(".loading-process").css("display", "inline");
    workingShiftService.QueryShiftModifyHistory(queryData, function (response) {
      $(".loading-process").css("display", "none");
      if (response) {
        var data = response;
        var newTableBody = modalBody.find(".log-table");
        newTableBody.append(data);
      } else {
        Swal.fire({
          html: "Something wrong!",
          type: "error",
          animation: false,
          heightAuto: false,
          scrollbarPadding: false,
          confirmButtonText: "OK",
        });
      }
    });
  }

  $.fn.copyWorkingShift = function () {
    this.click(function (event) {
      event.stopPropagation();
      hideAllPopup();
      workingShiftCore.ExitCopyShiftMode();
      var htmlRow = $(this).closest(".staff-row");
      var staffName = htmlRow.find(".staff-name").html();
      const messageConfirmText =
        "Do you want to copy " +
        staffName +
        "'s working shift from previous week?";
      Swal.fire({
        html: messageConfirmText,
        type: "question",
        showCancelButton: true,
        reverseButtons: true,
        animation: false,
        heightAuto: false,
        scrollbarPadding: false,
        confirmButtonText: "OK",
      }).then((result) => {
        if (result.value) {
          var userId = parseInt(htmlRow.attr("data-id"));
          var model = {
            userId: userId,
            startDateOfWeek: grid.fromDate,
          };
          // $('.loading-process').css('display', 'inline');
          workingShiftService
            .CopyWorkingShiftWeeklyPrevious(model)
            .success(function (data) {
              $(".loading-process").css("display", "none");
              updateWorkingShiftDataRow(userId, data, false);
              grid.rerenderDayBlock(userId);
              htmlRow.addClass("is-focus");
            })
            .error(function () {
              $(".loading-process").css("display", "none");
            });
        }
      });
    });
  };
  $.fn.workingShift = function () {
    function generateShiftDropdownHtml(shifts) {
      var htmlShiftCode = [];
      htmlShiftCode.push("<ul>");
      for (var i = 0; i < shifts.length; i++) {
        var shift = shifts[i];
        var shiftId = shift.ShiftId;
        var shiftCode = encodeShiftCode(shift.ShiftCode);
        var bgColor = shift.Background;
        var color = shift.Color;
        var isTimeShift = shift.IsTimeShift;
        htmlShiftCode.push(
          '<li class="js-edit-shift-code-item" data-istimeshift="' +
            isTimeShift +
            '" data-shift="' +
            shiftId +
            '" data-color="' +
            color +
            '" data-bgcolor="' +
            bgColor +
            '" >'
        );
        htmlShiftCode.push(
          '<span class="shift-color" style="background: ' +
            bgColor +
            "; color:" +
            color +
            ';"></span>'
        );
        htmlShiftCode.push(
          ' <span class="shift-text tooltip-tipsy" original-title="' +
            shiftCode +
            '">' +
            shiftCode +
            "</span>"
        );
        htmlShiftCode.push("</li>");
      }
      htmlShiftCode.push("</ul>");
      return htmlShiftCode.join("");
    }

    this.off("click").on("click", function (event) {
      event.stopPropagation();

      if (workingShiftData.isInCopyModeSingle) {
        var oldShiftId = workingShiftData.CopyWorkingShiftSingle.shiftId;
        var oldRefShiftId = workingShiftData.CopyWorkingShiftSingle.refShiftId;
        var oldEmployeeId =
          workingShiftData.CopyWorkingShiftSingle.fromEmployeeId;

        var dayBlockElement = $(event.target).closest(".day");
        var shiftBlockElement = $(event.target).closest(".js-edit-shift");
        var toEmployeeId = dayBlockElement.data("employeeid");
        var toDate = dayBlockElement.data("date");
        var sectionId = dayBlockElement.data("sectionid");
        var destShiftId = shiftBlockElement.data("shift");
        var destRefShiftId = shiftBlockElement.data("refshift");
        if (!canEditable(toEmployeeId, sectionId)) {
          Swal.fire({
            html: `You don't have permission to paste`,
            type: "warning",
            animation: false,
            heightAuto: false,
            scrollbarPadding: false,
          });
          return;
        }

        if (
          !isFutureWeek &&
          oldShiftId === destShiftId &&
          oldRefShiftId === destRefShiftId
        ) {
          dayBlockElement.addClass("is-focus");
          return;
        }
        var postModel = {
          ShiftId: oldShiftId > 0 ? oldShiftId : null,
          RefShiftId: oldRefShiftId > 0 ? oldRefShiftId : null,
          UserId: toEmployeeId,
          ShiftDate: toDate,
        };

        if (postModel.UserId && postModel.ShiftDate && postModel.ShiftId) {
          try {
            workingShiftService
              .CopySingleShift(postModel, function (res) {
                var data = res.Data;
                if (data) {
                  var htmlCell = renderOneCell({
                    refShiftId: data.RefShiftId,
                    user: { SectionId: sectionId, UserId: data.UserId },
                    thisDay: toDate,
                    shiftId: data.ShiftId,
                    isDefaultShift: false,
                  });

                  dayBlockElement.addClass("is-focus");
                  dayBlockElement
                    .html(htmlCell)
                    .find(".js-edit-shift")
                    .workingShift();
                }
              })
              .error(function (error) {
                var errorMessage =
                  "An error occurred when pasting, please try again.";
                Swal.fire({
                  html: errorMessage,
                  type: "error",
                  animation: false,
                });

                Raven.captureMessage(JSON.stringify(error));
              });
          } catch (e) {
            Raven.captureException(e);
          }
        }
      } else {
        var editShiftElement = $(this).closest(".js-edit-shift");
        var currentShiftId = parseInt(editShiftElement.attr("data-shift"));
        var currentRefShiftId = parseInt(
          editShiftElement.attr("data-refshift")
        );
        var hasShift = currentShiftId > 0;
        var isOffShift = false;
        if (hasShift) {
          var currentShift = findShiftById(currentShiftId);
          if (
            currentShift.ShiftCode === "PH" ||
            currentShift.ShiftCode === "OFF"
          ) {
            isOffShift = true;
          }
        }
        var hasOT = currentRefShiftId > 0;
        var isClickOnPopup = $(event.target).closest(".edit-shift").length > 0;
        var isDefaultWS =
          $(event.target).closest(".edit-default-shift").length > 0;
        if (!isClickOnPopup) {
          if (isDefaultWS) {
            $(this)
              .closest(".edit-default-shift")
              .find(".edit-shift")
              .removeClass("is-open")
              .empty();
          } else {
            hideAllPopup();
          }
        }
        event.stopPropagation();

        var elementRoot = $(this);
        var staffRowElement = $(this).closest(".staff-row");
        var userId = parseInt(staffRowElement.data("id"));
        var sectionId = parseInt(staffRowElement.data("sectionid"));
        if (!canEditable(userId, sectionId)) {
          return;
        }

        if (!isClickOnPopup) {
          var html = [];
          html.push(
            '<div class="title"><p>Change Shifts</p><i class="mdi mdi-close"></i><div class="clearfix"></div></div>'
          );

          html.push(
            '<div class="checkbox"><input class="filled-in checkbox" id="checkOT" name="checkOT" type="checkbox" /><label for="checkOT">Overtime</label></div>'
          );

          html.push(
            '<div class="input-field"><input type="text" class="input-shift validate" placeholder="Search" maxlength="50" autocomplete="off"><a href="javascript:void(0)" class="clear_shift_input"><i class="mdi mdi-close tooltip-tipsy" original-title="Cancel"></i></a></div><div class="js-shift-dropdown"></div>'
          );
          var htmlToAppend = html.join("");
          $(this).find(".edit-shift").html(htmlToAppend);
          $(this)
            .find(".mdi-close")
            .click(function (event) {
              event.stopPropagation();
              elementRoot.find(".edit-shift").removeClass("is-open").empty();
            });
          $(this).find(".tooltip-tipsy").tipsy({ gravity: "sw" });

          $(this).find(".edit-shift").toggleClass("is-open");

          var filterShowHideShift = function () {
            var shiftSearchText = $.trim(
              $(this).find(".input-shift").val()
            ).toUpperCase();
            var isOT = $(this).find("#checkOT").prop("checked");
            var shifts = workingShiftSetting.shifts;
            if (isOT) {
              shifts = shifts.filter((s) => s.IsTimeShift);
            }

            shifts = filterAndOrderShifts(shifts, shiftSearchText);
            var htmlShiftCode = generateShiftDropdownHtml(shifts);
            $(this)
              .parent()
              .parent()
              .find(".js-shift-dropdown")
              .html(htmlShiftCode);
            $(this)
              .parent()
              .parent()
              .find(".js-edit-shift-code-item")
              .click(function (event) {
                event.stopPropagation();
                var shiftId = parseInt($(this).data("shift"));
                var shiftCode = $(this).find(".shift-text").text();
                var getBgColor = $(this).data("bgcolor");
                var getColor = $(this).data("color");
                var dayBlockElement = $(this).closest(".day");
                var employeeId = parseInt(dayBlockElement.data("employeeid"));
                var editShiftElement = $(this).closest(".js-edit-shift");
                var currentShiftId = parseInt(editShiftElement.data("shift"));
                var currentRefShiftId = parseInt(
                  editShiftElement.data("refshift")
                );
                var shiftElement = $(this).closest(".shift");
                if (isDefaultWS) {
                  hideAllPopupIgnoreDefaultShift();
                  var dayOfTheWeek = parseInt(
                    editShiftElement.closest(".day").attr("data-index")
                  );
                  if (dayOfTheWeek === 0) {
                    var dayBlock = editShiftElement.closest(".day-block");
                    dayBlock.find(".shift").each(function (index, element) {
                      updateShift($(element));
                    });
                  } else {
                    updateShift(shiftElement);
                  }
                } else {
                  var shiftDate = dayBlockElement.data("date");
                  var refShiftId = null;
                  var isOvertime = $(elementRoot)
                    .find("#checkOT")
                    .prop("checked");
                  if (isOvertime) {
                    refShiftId = currentRefShiftId || currentShiftId;
                  }
                  var modelUpdate = {
                    EmployeeId: employeeId,
                    ShiftId: shiftId,
                    ShiftDate: shiftDate,
                    RefShiftId: refShiftId,
                    IsOvertime: isOvertime,
                  };
                  workingShiftService
                    .UpdateWorkingShiftCode(modelUpdate, function (res) {
                      hideAllPopup();
                      var data = res.Data;
                        var toDate = moment($(".weekpicker").data("endDate"), 'MM/DD/YYYY').add(
                        1,
                        "days"
                      );

                      if (
                        moment(data.ShiftDate).isoWeekday() === 1 &&
                        moment(data.ShiftDate).isSame(toDate, "day")
                      ) {
                        for (var i = 0; i < workingShiftUsers.length; i++) {
                          if (workingShiftUsers[i].UserId === data.UserId) {
                            workingShiftUsers[i].WorkingShifts[7] = data;
                            break;
                          }
                        }
                      }

                      var htmlCell = renderOneCell({
                        refShiftId: data.RefShiftId,
                        user: { SectionId: sectionId, UserId: data.UserId },
                        thisDay: data.ShiftDate,
                        shiftId: data.ShiftId,
                        isDefaultShift: false,
                      });

                      shiftElement
                        .parent()
                        .html(htmlCell)
                        .find(".js-edit-shift")
                        .workingShift();
                    })
                    .error(function () {
                      $(".loading-process").css("display", "none");
                    });
                }

                function updateShift(shiftElement) {
                  shiftElement.attr("data-shift", shiftId);
                  shiftElement.find("span:first").text(shiftCode);
                  shiftElement.find("span:first").attr("data-shift", shiftId);
                  shiftElement
                    .find("span:first")
                    .css({ background: getBgColor, color: getColor });
                  shiftElement.find(".edit-shift").next().remove();
                  var buildClearIcon =
                    '<i class="mdi mdi-close icon-clear-shift js-clear-shift" data-shift="' +
                    shiftId +
                    '" data-employeeid="' +
                    userId +
                    '" data-date="' +
                    shiftDate +
                    '"></i>';
                  shiftElement.append(buildClearIcon);
                  shiftElement
                    .find(".js-clear-shift")
                    .click(onRemoveShiftClicked);
                  $(".tipsy").remove();
                }
              });
          };

          // handle textbox to input shift
          $(this).find(".input-shift").focus();
          $(this).find(".input-shift").off("input");
          $(this)
            .find(".input-shift")
            .on("input", function (event) {
              var shiftSearchText = $.trim($(this).val()).toUpperCase();
              if (shiftSearchText.length) {
                $(this).closest("div").find(".clear_shift_input").show();
              } else {
                $(this).closest("div").find(".clear_shift_input").hide();
              }
              filterShowHideShift.call(elementRoot);
            });

          $(this).find(".input-shift").trigger("input");

          // handle checkbox OT
          if (isDefaultWS || (!hasOT && (!hasShift || !isOffShift))) {
            $(this).find("#checkOT").parent().hide();
          }
          $(this)
            .find("#checkOT")
            .change(function () {
              filterShowHideShift.call(elementRoot);
            });
          if (hasOT) {
            $(this).find("#checkOT").prop("checked", true);
            $(this).find("#checkOT").prop("disabled", true);
            filterShowHideShift.call(elementRoot);
          }

          $(this)
            .find(".clear_shift_input")
            .click(function () {
              $(this).closest("div").find("input").val("");
              $(this).closest("div").find("input").trigger("input");
            });
        }
      }
    });
    $(".js-clear-shift").off("click").on("click", onRemoveShiftClicked);
    function onRemoveShiftClicked(event) {
      event.stopPropagation();
      var target = $(this);
      var isEditDefaultShift = target.closest(".edit-default-shift").length > 0;
      if (isEditDefaultShift) {
        hideAllPopupIgnoreDefaultShift();
      } else {
        hideAllPopup();
      }

      var staffRowElement = target.closest(".staff-row");
      var userId = parseInt(staffRowElement.data("id"));
      var sectionId = parseInt(staffRowElement.data("sectionid"));
      if (!canEditable(userId, sectionId)) {
        return;
      }

      var removeShift = function (shiftElement) {
        shiftElement.find("span:first").text("");
        shiftElement.css({ background: "#fff", color: "#fff" });
        shiftElement.attr("data-shift", "");
        shiftElement.remove(".js-clear-shift");
        shiftElement
          .find(".shift-name")
          .css({ background: "#fff", color: "#fff" });
        shiftElement.find(".shift-name").attr("data-shift", "");
        shiftElement.find(".js-clear-shift").remove();
        shiftElement.find(".js-copy-single-shift").remove();
      };

      var dayOfTheWeek = parseInt(target.closest(".day").attr("data-index"));
      var shiftElement = target.closest(".shift");
      if (isEditDefaultShift) {
        if (dayOfTheWeek === 0) {
          var dayBlock = target.closest(".day-block");
          dayBlock.find(".shift").each(function (index, element) {
            removeShift($(element));
          });
        } else {
          removeShift(shiftElement);
        }
      } else {
        var shiftCode = target.parent().find(".shift-name").last().text();
        var messageConfirmText =
          "Do you want to remove shift <br /> <b class='shift-name'>" +
          shiftCode.toString().toUpperCase() +
          " ?</b>";

        Swal.fire({
          html: messageConfirmText,
          type: "warning",
          showCancelButton: true,
          reverseButtons: true,
          animation: false,
          heightAuto: false,
          scrollbarPadding: false,
          confirmButtonText: "OK",
        }).then((result) => {
          if (result.value) {
            var dayBlockElement = target.closest(".day");
            var userId = parseInt(dayBlockElement.data("employeeid"));
            var shiftDate = dayBlockElement.data("date");

            var model = {
              EmployeeId: userId,
              ShiftDate: shiftDate,
            };

            workingShiftService
              .RemoveWorkingShiftCode(model, function (res) {
                hideAllPopup();
                removeShift(shiftElement);
                var data = res.Data;
                if (data) {
                  var sectionId = shiftElement.data("sectionid");
                  var toDate = shiftElement.data("date");
                  var htmlCell = renderOneCell({
                    refShiftId: data.RefShiftId,
                    user: { SectionId: sectionId, UserId: model.EmployeeId },
                    thisDay: toDate,
                    shiftId: data.ShiftId,
                    isDefaultShift: false,
                  });

                  shiftElement
                    .parent()
                    .html(htmlCell)
                    .find(".js-edit-shift")
                    .workingShift();
                } else {
                  if (
                    isFutureWeek ||
                    moment(shiftDate).isSame(moment(grid.toDate), "day")
                  ) {
                    var userDefaultShiftIds = findDefaultShiftIds(userId) || {};
                    var defaultShiftId = userDefaultShiftIds[dayOfTheWeek - 1];
                    var sectionId = shiftElement.data("sectionid");
                    var toDate = shiftElement.data("date");
                    if (defaultShiftId) {
                      var htmlCell = renderOneCell({
                        user: {
                          SectionId: sectionId,
                          UserId: model.EmployeeId,
                        },
                        thisDay: toDate,
                        shiftId: defaultShiftId,
                        isDefaultShift: true,
                      });
                      shiftElement
                        .parent()
                        .html(htmlCell)
                        .find(".js-edit-shift")
                        .workingShift();
                    } else if (dayOfTheWeek === 8) {
                      var htmlCell = renderOneCell({
                        user: {
                          SectionId: sectionId,
                          UserId: model.EmployeeId,
                        },
                        thisDay: toDate,
                        shiftId: userDefaultShiftIds[0],
                        isDefaultShift: true,
                      });
                      shiftElement
                        .parent()
                        .html(htmlCell)
                        .find(".js-edit-shift")
                        .workingShift();
                    }
                  }
                }
              })
              .error(function () {
                $(".loading-process").css("display", "none");
              });
          }
        });
      }
    }

    $(".js-copy-single-shift").off("click").on("click", onCopySingleShift);

    function onCopySingleShift(event) {
      event.stopPropagation();
      workingShiftCore.ExitCopyShiftMode();
      hideAllPopup();
      if (!isHost) {
        return;
      }

      toastr.info("Press ESC to exit Copy shift mode");

      var target = $(this);
      var boxParent = $(target).closest(".js-edit-shift");
      if (boxParent) {
        $(boxParent).parent().addClass("is-copied");
      }
      var shiftId = $(boxParent).data("shift");
      var refShiftId = $(boxParent).data("refshift");
      var fromEmployeeId = $(boxParent).data("employeeid");
      var fromDate = $(boxParent).data("date");

      workingShiftData.isInCopyModeSingle = true;
      workingShiftData.CopyWorkingShiftSingle = {
        refShiftId: refShiftId,
        shiftId: shiftId,
        fromEmployeeId: fromEmployeeId,
        fromDate: fromDate,
      };
    }
  };
  $.fn.editDefaultShift = function () {
    this.click(function (event) {
      event.stopPropagation();
      workingShiftCore.ExitCopyShiftMode();
      hideAllPopup();
      var staffRow = $(this).closest(".staff-row");
      var staffName = staffRow.find(".staff-name > a");
      var container = staffRow.find(".edit-default-shift");
      var userId = parseInt(staffRow.attr("data-id"));
      var userDefaultShiftIds = findDefaultShiftIds(userId) || {};
      var date = new Date();
      var year = date.getFullYear(),
        month = date.getMonth(),
        day = date.getDate() - date.getDay() + 1;

      var startDate = new Date(year, month, day);
      var endDate = new Date(year, month, day + 6);

      var startDateString = moment(startDate).format("MMM DD, YYYY");
      var endDateString = moment(endDate).format("MMM DD, YYYY");

      var template = [];
      template.push(
        '<div class="title">Default Shifts Setting <b>' +
          staffName.text().trim() +
          '</b><i class="mdi mdi-close js-close"></i></div>'
      );
      template.push(
        '\
                <div class="header">\
                    <div class="day-block">\
                        <div class="day">DEFAULT SHIFT</div>\
                        <div class="day">MON</div>\
                        <div class="day">TUE</div>\
                        <div class="day">WED</div>\
                        <div class="day">THU</div>\
                        <div class="day">FRI</div>\
                        <div class="day">SAT</div>\
                        <div class="day">SUN</div>\
                    </div>\
                </div>'
      );
      template.push('<div class="content"><div class="day-block">');
      for (var i = 0; i <= 7; i++) {
        var shiftCode = "";
        var shiftId = "";
        var color = "";
        var background = "";
        if (i > 0) {
          var defaultShiftId = userDefaultShiftIds[i - 1];
          if (defaultShiftId) {
            shiftId = defaultShiftId;
            var shift = findShiftById(shiftId);
            if (shift) {
              shiftCode = shift.ShiftCode;
              color = shift.Color;
              background = shift.Background;
            }
          }
        }

        template.push(
          '<div data-index="' +
            i +
            '" class="day"><div class="shift js-edit-shift" data-shift="' +
            shiftId +
            '">'
        );
        template.push(
          '<span class="shift-name" style="background: ' +
            background +
            "; color:" +
            color +
            ' " data-shift="' +
            shiftId +
            '">' +
            shiftCode +
            "</span>"
        );
        template.push(
          '<div class="edit-shift" data-employeeid=' + userId + "></div>"
        );
        if (shiftId > 0) {
          template.push(
            '<i class="mdi mdi-close icon-clear-shift js-clear-shift" data-shift="' +
              shiftId +
              '" data-employeeid="' +
              userId +
              '"></i>'
          );
        }
        template.push("</div></div>");
      }
      template.push("</div></div>");
      template.push(
        '<p style="margin-top: 5px;font-style:italic;">Note: The Default Shift runs every Friday at 00:00 (GMT+7) to apply for next week</p>'
      );
      template.push(
        '<div class="checkbox-apply"><input class="filled-in checkbox js-applyCurrentWeek" id="applyCurrentWeek" name="applyCurrentWeek" type="checkbox" value="true"><label for="applyCurrentWeek">Apply default shifts for current week (' +
          '<span class="txt-bold">' +
          startDateString +
          " - " +
          endDateString +
          "</span>" +
          ")</div>"
      );
      template.push('<div class="button-block">');
      template.push(
        '<a class="waves-effect waves-light btn btn-cancel">Cancel</a>'
      );
      template.push(
        '<a class="waves-effect waves-light btn btn-search js-button-save">Apply</a>'
      );
      template.push(
        '<span class="js-update-status success-info">Updated successfully</a>'
      );
      template.push("</div>");
      template.push("</div>");

      container.html(template.join(""));
      container.css({
        left: $(this).position().left,
      });
      if (!container.hasClass("is-open")) {
        container.addClass("is-open");
      }
      container.click((event) => {
        event.stopPropagation();
      });
      container.find(".btn-cancel").click(function () {
        hideAllPopup();
      });
      container.find(".js-close").click(function () {
        hideAllPopup();
      });
      container.find(".js-update-status").hide();
      container.find(".js-button-save").click(function () {
        workingShiftCore.ExitCopyShiftMode();
        var dayBlock = container.find(".day-block");
        var defaultShiftIds = [];
        dayBlock.find(".day").each(function (index, element) {
          var dayOfTheWeek = parseInt($(element).attr("data-index"));

          if (dayOfTheWeek > 0) {
            var defaultShiftId = parseInt(
              $(element).find(".js-edit-shift").attr("data-shift")
            );
            if (defaultShiftId) {
              defaultShiftIds.push(defaultShiftId);
            } else {
              defaultShiftIds.push(undefined);
            }
          }
        });
        var model = {
          employeeId: userId,
          defaultShiftIds: defaultShiftIds.join(","),
          isApplyCurrentWeek: container
            .find("#applyCurrentWeek")
            .prop("checked"),
          fromDate: startDateString,
          toDate: endDateString,
        };
        $(".loading-process").css("display", "inline");
        container.find(".js-update-status").hide();
        workingShiftService.UpdateDefaultShift(model, function () {
          var isChecked =
            container.find(".js-applyCurrentWeek:checkbox:checked").length > 0;

          var dtPicker = $(".weekpicker").val();
          var dtArray = dtPicker.split("-");
          var fromDate = dtArray[0].trim();
          var toDate = dtArray[1].trim();
          model = {
            UserId: userId,
            fromDate: fromDate,
            toDate: toDate,
          };

          workingShiftService.DefaultShiftGetWorkingShiftByUser(
            model,
            function (result) {
              updateDefaultShiftIds(userId, defaultShiftIds);
              container.find(".js-update-status").show();

              updateWorkingShiftDataRow(userId, result, false);
              grid.rerenderDayBlock(userId);
              $(".loading-process").css("display", "none");
            }
          );

          $(".loading-process").css("display", "none");
        });
      });
      $(".edit-default-shift .js-edit-shift").workingShift();
    });
  };

  $.fn.editShiftRemark = function () {
    this.click(function (event) {
      workingShiftCore.ExitCopyShiftMode();
      hideAllPopup();
      event.stopPropagation();

      var staffRowElement = $(this).closest(".staff-row");
      var userId = parseInt(staffRowElement.data("id"));
      var sectionId = parseInt(staffRowElement.data("sectionid"));
      if (!canEditable(userId, sectionId)) {
        return;
      }

      var $this = this;
      $(".remark-popover").removeClass("is-open");

      var remarkContainer = $(this).parent();
      var remarkPopover = remarkContainer.next();
      remarkPopover.toggleClass("is-open");
      var remarkContent = remarkContainer.find(".show-remark").text();
      var textarea = remarkPopover.find("textarea");
      textarea.val(remarkContent);
      textarea.focusin();

      var closeButton = remarkPopover.find(".mdi-close");
      closeButton.off("click");
      closeButton.on("click", function () {
        remarkPopover.toggleClass("is-open");
      });
      var cancelButton = remarkPopover.find(".btn-cancel");
      cancelButton.off("click");
      cancelButton.on("click", function () {
        remarkPopover.toggleClass("is-open");
      });
      var saveButton = remarkPopover.find(".btn-save");
      saveButton.off("click");
      saveButton.on("click", function () {
        var htmlRow = $(this).closest(".staff-row");
        var userId = parseInt(htmlRow.attr("data-id"));
        var model = {
          apiKey: "working-shift-update-remark-weekly",
          userId: userId,
          category: 1,
          remark: textarea.val(),
        };
        $(".loading-process").css("display", "inline");
        workingShiftService
          .UpdateWorkingShiftRemark(model, function () {
            $(".loading-process").css("display", "none");
            remarkContainer.find(".show-remark").text(model.remark);
            remarkContainer
              .find(".show-remark")
              .attr("original-title", model.remark);
            $($this).removeClass("has-remark");
            if (model.remark) {
              $($this).addClass("has-remark");
            }
            remarkPopover.toggleClass("is-open");
          })
          .error(function () {
            $(".loading-process").css("display", "none");
            toastr.error("Update failure");
          });
      });
    });
  };

  $.fn.onViewLog = function () {
    this.click(function (event) {
      var _this = this;
      var objectId = $(_this).closest(".staff-row").data("id");
      var username = $(_this)
        .closest(".staff-row")
        .find(".js-traderName")
        .text();
      $("#js-modalViewLog").attr("data-objectId", objectId);
      $("#js-modalViewLog")
        .find(".title")
        .text("History of " + username);
      $("#js-modalViewLog").openModal({
        dismissible: false,
        ready: function () {
          $(":input, a").attr("tabindex", "-1");
          $(".modal-action").focus();
        },
        complete: function () {
          $(":input, a").removeAttr("tabindex");
          $(".lean-overlay").each(function () {
            $(this).remove();
          });
        },
      });

      queryHistoryLog(50, objectId);
    });
  };

  $.fn.onCloseHistoryLogModal = function () {
    this.click(function (event) {
      var model = $("#js-modalViewLog");
      var dropdown = model.find("#action-number");
      dropdown.val("50");
      dropdown.material_select();
    });
  };

  $.fn.onChangeHistoryModalSelection = function () {
    this.change(function (event) {
      var _this = this;
      var modal = $(_this).closest("#js-modalViewLog");
      var objectId = modal.attr("data-objectId");
        queryHistoryLog(event.target.value, objectId);
        this.off('change');
    });
  };

  function WorkingShiftSetting(shifts) {
    this.shifts = shifts;
  }

  WorkingShiftSetting.prototype.render = function (shifts) {
    hideAllPopupIgnoreWorkingShiftColor();

    workingShiftCore.ExitCopyShiftMode();

    if (this.shifts.length === 0) {
      $(".option-config").hide();
      $(".working-shift-list").hide();
      return;
    }

    if (!shifts) {
      shifts = this.shifts;
    }

    $(".option-config").show();
    $(".working-shift-list").show();
    var html = [];
    for (var i = 0; i < shifts.length; i++) {
      var shift = shifts[i];
      var shiftCode = encodeShiftCode(shift.ShiftCode);
      html.push("<li>");
      html.push(
        '<div class="working-shift-item" data-shift="' +
          shift.ShiftId +
          '" data-background="' +
          shift.Background +
          '" data-color="' +
          shift.Color +
          '">'
      );
      html.push('<span class="shift-color"></span>');
      html.push(
        '<span class="shift-text tooltip-tipsy" original-title="' +
          shiftCode +
          '">' +
          shiftCode +
          "</span>"
      );
      if (isHostShiftColor) {
        html.push(
          '<span class="shift-delete-item"><i class="mdi mdi-delete tooltip-tipsy" original-title="Delete"></i></span>'
        );
      }
      html.push("</div>");
      html.push("</li>");
    }
    $(".working-shift-list").html(html.join(""));
    $(".working-shift-list .tooltip-tipsy").tipsy({ gravity: "sw" });
    this.updateColorItem();
  };

    WorkingShiftSetting.prototype.initEvents = function () {
    $("#first_name2")
      .off("input")
      .on("input", function () {
        $(".format-style").removeClass("is-open");
        var shiftSearchText = $("#first_name2").val();
        if (shiftSearchText) {
          shiftSearchText = shiftSearchText
            .toString()
            .trim(shiftSearchText)
            .toUpperCase();
        }
        if (shiftSearchText) {
          $(".group-button").show();
        } else {
          $(".group-button").hide();
          }
        var shifts = filterAndOrderShifts(
          workingShiftSetting.shifts,
          shiftSearchText
        );
        workingShiftSetting.render(shifts);
        workingShiftSetting.handleUpdateColor();
      });

    $(".clear_shift")
      .off("click")
      .on("click", function () {
        $("#first_name2").val("");
        $("#first_name2").trigger("input");
        $("#first_name2").focus();
      });

    $("#first_name2")
      .off("keypress")
      .on("keypress", function (event) {
        var keycode = event.keyCode ? event.keyCode : event.which;
        if (keycode == "13") {
          var shiftCode = $.trim($("#first_name2").val()).replace(/\s+/g, " ");
          if (shiftCode) {
            $(".addnew_shift").click();
          }
        }
        event.stopPropagation();
      });

    $(".addnew_shift")
      .off("click")
      .on("click", function () {
        if (!isHostShiftColor) {
          return;
        }
        var shiftCode = $.trim($("#first_name2").val()).replace(/\s+/g, " ");
        $("#first_name2").val(shiftCode);
        $("#first_name2").trigger("input");
        if (shiftCode) {
          if (shiftCode.indexOf("<") > -1 || shiftCode.indexOf(",") > -1) {
            const messageConfirmText =
              "Shift should not contain specific characters like < or ,";
            Swal.fire({
              html: messageConfirmText,
              type: "error",
              animation: false,
              heightAuto: false,
              scrollbarPadding: false,
              confirmButtonText: "OK",
            });
            return;
          }

          var findIndex = -1;
          for (i = 0; i < workingShiftSetting.shifts.length; i++) {
            if (workingShiftSetting.shifts[i].ShiftCode === shiftCode) {
              findIndex = i;
              break;
            }
          }
          if (findIndex > -1) {
            const messageConfirmText = "Shift existed";
            Swal.fire({
              html: messageConfirmText,
              type: "error",
              animation: false,
              heightAuto: false,
              scrollbarPadding: false,
              confirmButtonText: "OK",
            });
            return;
          }

          var quickInsertWorkingShiftColor = function (shiftCode) {
            var shift = {
              Background: "#3399cc",
              Color: "#ffffff",
              ShiftCode: shiftCode,
            };

            workingShiftService.QuickInsertWorkingShiftColor(
              shift,
              function (response) {
                if (response.IsSuccess) {
                  var data = response.Data;
                  var newShift = {
                    ShiftId: data.ShiftId,
                    ShiftCode: data.ShiftCode,
                    Color: data.Color,
                    Background: data.Background,
                  };
                  workingShiftSetting.shifts.unshift(newShift);
                  $("#first_name2").val(data.ShiftCode);
                  $("#first_name2").trigger("input");
                  $(".js-quickaccess").addClass("is-open");
                } else {
                  Swal.fire({
                    html: response.Message,
                    type: "error",
                    animation: false,
                    heightAuto: false,
                    scrollbarPadding: false,
                    confirmButtonText: "OK",
                  });
                }

                if (data) {
                  $("#first_name2").val(data.ShiftCode);
                  $("#first_name2").trigger("input");
                }
              }
            );
          };

          workingShiftService.ValidateBeforeCreateWorkingShiftColor(
            { ShiftCode: shiftCode },
            function (data) {
              if (data.Code === 0) {
                quickInsertWorkingShiftColor(shiftCode);
              } else if (data.Code < 0) {
                Swal.fire({
                  html: data.Message,
                  type: "error",
                  animation: false,
                  heightAuto: false,
                  scrollbarPadding: false,
                  confirmButtonText: "OK",
                });
              } else {
                Swal.fire({
                  html: data.Message,
                  type: "question",
                  showCancelButton: true,
                  reverseButtons: true,
                  animation: false,
                  heightAuto: false,
                  scrollbarPadding: false,
                  confirmButtonText: "OK",
                }).then((result) => {
                  if (result.value) {
                    quickInsertWorkingShiftColor(shiftCode);
                  }
                });
              }
            }
          );
        }
      });

    $("#bg-color").colpick({
      onSubmit: function (hsb, hex, rgb, el, bySetColor) {
        $(el).val("#" + hex);
        $(".shift-style-demo").css("background", "#" + hex);
        $(el).colpickHide();
      },
    });

    $("#text-color").colpick({
      onSubmit: function (hsb, hex, rgb, el, bySetColor) {
        $(el).val("#" + hex);
        $(".shift-style-demo").css("color", "#" + hex);
        $(el).colpickHide();
      },
    });

    $("#btn-cancel-color")
      .off("click")
      .on("click", function () {
        $(".format-style").removeClass("is-open");
        $(".working-shift-item").removeClass("active");
      });
    $("#btn-update-color")
      .off("click")
      .on("click", function () {
        var shift = {
          ShiftId: parseInt($(".shift-style-demo").attr("data-shift")),
          Background: $("#bg-color").val(),
          Color: $("#text-color").val(),
        };
        $(".loading-process").css("display", "inline");
        workingShiftService
          .UpdateShiftColor(shift, function (obj) {
            $(".loading-process").css("display", "none");
            $(".working-shift-item").removeClass("active");
            $(".format-style").removeClass("is-open");
            var elementSelector =
              '.working-shift-item[data-shift="' + shift.ShiftId + '"]';
            var changeShiftColor = $(".day").find(
              '.shift-name[data-shift="' + shift.ShiftId + '"]'
            );

            $(elementSelector).attr("data-background", shift.Background);
            $(elementSelector).attr("data-color", shift.Color);
            changeShiftColor.css({
              background: shift.Background,
              color: shift.Color,
            });
            $.each(workingShiftSetting.shifts, function (index, x) {
              if (x.ShiftId === shift.ShiftId) {
                x.Background = shift.Background;
                x.Color = shift.Color;
              }
            });
            workingShiftSetting.updateColorItem();
            // $('.js-edit-shift').workingShift();
          })
          .error(function (a, b, c) {
            $(".loading-process").css("display", "none");
          });
      });
  };

  WorkingShiftSetting.prototype.updateColorItem = function () {
    $(".working-shift-list .working-shift-item .shift-color").each(function (
      index,
      element
    ) {
      $(element).css({
        background: $(this).parent().attr("data-background"),
        color: $(this).parent().attr("data-color"),
      });
    });
  };

  WorkingShiftSetting.prototype.handleUpdateColor = function () {
    $(".shift-delete-item").click(function (event) {
      event.stopPropagation();
      workingShiftCore.ExitCopyShiftMode();

      if (!isHostShiftColor) {
        return;
      }
      $(".format-style").removeClass("is-open");
      var shiftId = parseInt($(this).parent().data("shift"));
      var shiftCode = $(this).parent().find(".shift-text").text();

      workingShiftService
        .ValidateBeforeDeleteWorkingShiftColor({ shiftId: shiftId })
        .success(function (response) {
          if (response.IsSuccess) {
            var messageConfirmText =
              "Do you want to delete shift <br /> <b class='shift-name'>" +
              shiftCode.toString().toUpperCase() +
              " ?</b>";
            Swal.fire({
              html: messageConfirmText,
              type: "question",
              showCancelButton: true,
              reverseButtons: true,
              animation: false,
              heightAuto: false,
              scrollbarPadding: false,
              confirmButtonText: "OK",
            }).then((result) => {
              if (result.value) {
                workingShiftService
                  .QuickDeleteWorkingShiftColor({ shiftId: shiftId })
                  .success(function (response) {
                    if (response.IsSuccess) {
                      var findIndex = -1;
                      for (i = 0; i < workingShiftSetting.shifts.length; i++) {
                        if (workingShiftSetting.shifts[i].ShiftId === shiftId) {
                          findIndex = i;
                          break;
                        }
                      }

                      workingShiftSetting.shifts.splice(findIndex, 1);
                      workingShiftSetting.render();
                      workingShiftSetting.handleUpdateColor();

                      $(".loading-process").css("display", "none");
                      $("#first_name2").trigger("input");
                      $(".js-quickaccess").addClass("is-open");
                    } else {
                      var errorMessage =
                        "You cannot delete the assigned shift </br><b>" +
                        shiftCode +
                        "</b>";
                      Swal.fire({
                        html: errorMessage,
                        type: "error",
                        animation: false,
                      });
                    }
                  })
                  .error(function (error) {
                    var errorMessage =
                      "An error occurred when deleting, please try again.";
                    Swal.fire({
                      html: errorMessage,
                      type: "error",
                      animation: false,
                    });
                  });
              }
            });
          } else {
            var errorMessage =
              "You cannot delete the assigned shift </br><b>" +
              shiftCode +
              "</b>";
            Swal.fire({
              html: errorMessage,
              type: "error",
              animation: false,
            });
          }
        });
    });

    $(".working-shift-item").click(function (event) {
      if ($(this).hasClass("active")) {
        $(this).removeClass("active");
        $(".format-style").removeClass("is-open");
        return;
      }

      $(".working-shift-item").removeClass("active");
      $(".format-style").addClass("is-open");
      $(this).addClass("active");
      $(".shift-style-demo").text($(this).text());
      $(".shift-style-demo").attr("data-shift", $(this).attr("data-shift"));
      $(".shift-style-demo").css("background", $(this).attr("data-background"));
      $(".shift-style-demo").css("color", $(this).attr("data-color"));
      $("#bg-color").val($(this).attr("data-background"));
      $("#bg-color")
        .colpick()
        .colpickSetColor($(this).attr("data-background"), true);
      $("#text-color").val($(this).attr("data-color"));
      $("#text-color")
        .colpick()
        .colpickSetColor($(this).attr("data-color"), true);
    });
  };

  function Grid(fromDate, toDate, dataUsers, listSectionPublish) {
    this.fromDate = fromDate;
    this.toDate = toDate;
    this.data = dataUsers;
    this.listSectionPublish = listSectionPublish;
  }

  function newGuid() {
    var sGuid = "";
    for (var i = 0; i < 32; i++) {
      sGuid += Math.floor(Math.random() * 0xf).toString(0xf);
    }
    return sGuid;
  }

  function generateContactInfo(user, dropdownId) {
    var html = [];
    html.push(
      '<ul id="' +
        dropdownId +
        '" class="dropdown-content" style="width: auto">'
    );
    /*
        var skype = user.Skype;
        if (skype) {
            var skypeChat = "skype:" + skype + "?chat";
            html.push(
                '<li> \
                        <a class="link-skype" href="' + skypeChat + '"> \
                            <i class="mdi mdi-skype icon-skype"></i> \
                            <span class="skype-value">' + skype + '</span> \
                        </a> \
                    </li>'
            );
        } else {
            html.push('<li><a href="javascript:;"><i class="mdi mdi-skype icon-skype"></i></a></li>');
        }
        */
    var extension = user.Extension;
    if (extension) {
      html.push(
        '<li> \
                        <a href="javascript:;"> \
                            <i class="mdi mdi-deskphone icon-phone"></i> \
                            <span class="extension-value">' +
          extension +
          "</span> \
                        </a> \
                    </li>"
      );
    } else {
      html.push(
        '<li><a href="javascript:;"><i class="mdi mdi-deskphone icon-phone"></i></a></li>'
      );
    }
    var phone = user.Phone;
    if (phone) {
      html.push(
        '<li> \
                        <a href="javascript:;"> \
                            <i class="mdi mdi-cellphone-iphone icon-phone"></i> \
                            <span class="phone-value">' +
          phone +
          "</span> \
                        </a> \
                    </li>"
      );
    } else {
      html.push(
        '<li><a href="javascript:;"><i class="mdi mdi-cellphone-iphone icon-phone"></i></a></li>'
      );
    }
    var email = user.Email;
    if (email) {
      var emailLink = "mailto:" + email + "";
      html.push(
        '<li> \
                        <a class="link-email" href="' +
          emailLink +
          '"> \
                            <i class="mdi mdi-email-outline icon-email"></i> \
                            <span class="email-value">' +
          email +
          "</span> \
                        </a> \
                    </li>"
      );
    } else {
      html.push(
        '<li><a href="javascript:;"><i class="mdi mdi-email-outline icon-email"></i></a></li>'
      );
    }
    html.push("</ul>");
    return html.join("");
  }

  function generateUserNameHasContact(user, dropdownId) {
    return (
      '<span class="staff-name txt-bold"> \
                    <a class="dropdown-button js-traderName" href="javascript:;" data-activates="' +
      dropdownId +
      '">' +
      user.DisplayName +
      '</a> \
                </span> \
                <span style="display:none">' +
      user.Nickname +
      "</span>"
    );
  }

  Grid.prototype.generateUserHtml = function (user) {
    var html = [];
    var userRemark = user.Remark === null ? "" : user.Remark;
    html.push(
      '<div class="staff-row" data-id="' +
        user.UserId +
        '" data-sectionid="' +
        user.SectionId +
        '">'
    );
    html.push('<div class="staff">');
    html.push('<div class="staff-item">');
    html.push(
      '<div class="avatar light-blue"><i class="mdi mdi-account"></i></div>'
    );
    html.push('<div class="staff-info">');

    var dropdownId = "dropdown" + newGuid();
    // html.push('<p><span class="staff-name txt-bold">' + user.Name + '</span></p>');
    console.log(isIFrame);
    html.push(generateUserNameHasContact(user, dropdownId));
    !isIFrame && html.push(generateContactInfo(user, dropdownId));

    var remarkIconClass = "icon-remark mdi mdi-message-text tooltip-tipsy";
    if (userRemark) {
      remarkIconClass += " has-remark";
    }

    html.push(
      '<p class="remark-container"><i class="' +
        remarkIconClass +
        '" original-title="Remark"></i><span class="show-remark tooltip-tipsy" original-title="' +
        userRemark +
        '">' +
        userRemark +
        "</span></p>"
    );
    html.push('<div class="remark-popover">');
    html.push(
      '<div class="input-field"><div class="label">Remark</div><textarea class="materialize-textarea" maxlength="200"></textarea></div>'
    );
    html.push('<i class="icon-close-popover mdi mdi-close"></i>');
    html.push(
      '<div class="btn-group"><a class="waves-effect waves-light btn btn-cancel">Cancel</a><a class="waves-effect waves-light btn btn-save" data-id="' +
        user.UserId +
        '">Save</a></div>'
    );
    html.push("</div>");
    html.push("</div>");
    html.push("</div>");
    html.push('<div class="staff-action">');
    if (canEditable(user.UserId, user.SectionId)) {
      if (workingShiftData.isInOrderMode) {
        html.push('<div class="customize-order-user">');
        html.push(
          '<i class="icon icon-sortable tooltip-tipsy" original-title="Customize order user"></i>'
        );
        html.push(
          '<div class="sortale-control"><span class="action js-orderUp">Up</span><span class="action js-orderDown">Down</span><span class="action js-orderTop">Top</span><span class="action js-orderBottom">Bottom</span></div>'
        );
        html.push("</div>");
      }

      html.push(
        '<i class="icon icon-default-shift tooltip-tipsy" original-title="Edit default shift"></i>'
      );
      html.push('<div class="edit-default-shift"></div>');
      html.push('<div class="more-action-function">');
      html.push('<i class="icon mdi mdi-dots-horizontal"></i>');
      html.push('<div class="action">');
      html.push(
        '<span class="action-item js-copyFromPreviousWeek"><i class="icon icon-clone-week mdi mdi-content-duplicate"></i>Copy from previous week</span>'
      );
      html.push(
        '<span class="action-item js-copyWholeShifts"><i class="icon icon-copy-shift mdi mdi-content-copy"></i>Copy shift</span>'
      );
      html.push(
        '<span class="action-item js-deleteWholeShifts"><i class="icon icon-delete mdi mdi-delete"></i>Delete shift</span>'
      );
      html.push("</div>");
      html.push("</div>");
      html.push(
        '<a class="mdi mdi-content-paste icon-paste js-pasteWholeShifts tooltip-tipsy" style="display: none" original-title="Click here to paste"></a>'
      );
    }
    html.push("</div>");
    html.push("</div>");
    html.push(grid.generateDayBlock(user));
    if ($(".body-content").attr("data-isIFrame") === "False")
      html.push(grid.generateModifyBy(user));
    html.push("</div>");

    return html.join("");
  };

  function renderOneCell(data) {
    var html = [];
    var name = "";
    var background = "";
    var color = "";

    if (data.shiftId) {
      var shift = findShiftById(data.shiftId);
      if (shift) {
        name = encodeShiftCode(shift.ShiftCode);
        background = shift.Background;
        color = shift.Color;
      }
    }
    if (data.refShiftId > 0) {
      var refShift = findShiftById(data.refShiftId);
      var refShiftName = encodeShiftCode(refShift.ShiftCode);
      var colorOT = refShift.Color;
      var backgroundOT = refShift.Background;
      if (data.isDefaultShift) {
        html.push(
          '<div class="shift shift-is-default js-edit-shift has-OT" data-shift="' +
            data.shiftId +
            '" data-refshift="' +
            data.refShiftId +
            '">'
        );
        html.push(
          '<span class="shift-name shift-off shift-is-default" style="background: ' +
            backgroundOT +
            "; color:" +
            colorOT +
            '">' +
            refShiftName +
            "</span>"
        );
        html.push(
          '<span class="shift-name shift-OT shift-is-default" style="background: ' +
            background +
            "; color:" +
            color +
            '">' +
            name +
            "</span>"
        );
      } else {
        html.push(
          '<div class="shift js-edit-shift has-OT" data-shift="' +
            data.shiftId +
            '" data-refshift="' +
            data.refShiftId +
            '">'
        );
        html.push(
          '<span class="shift-name shift-off" style="background: ' +
            backgroundOT +
            "; color:" +
            colorOT +
            '">' +
            refShiftName +
            "</span>"
        );
        html.push(
          '<span class="shift-name shift-OT" style="background: ' +
            background +
            "; color:" +
            color +
            '">' +
            name +
            "</span>"
        );
      }
    } else {
      if (data.isDefaultShift) {
        if (data.shiftId && data.shiftId > 0) {
          html.push(
            '<div class="shift js-edit-shift shift-is-default" data-shift="' +
              data.shiftId +
              '" data-refshift="' +
              data.refShiftId +
              '">'
          );
          html.push(
            '<span class="shift-name shift-is-default" style="background:' +
              background +
              "; color:" +
              color +
              '">' +
              name +
              "</span>"
          );
        } else {
          html.push(
            '<div class="shift js-edit-shift" data-shift="' +
              data.shiftId +
              '" data-refshift="' +
              data.refShiftId +
              '">'
          );
          html.push(
            '<span class="shift-name" style="background:' +
              background +
              "; color:" +
              color +
              '">' +
              name +
              "</span>"
          );
        }
      } else {
        html.push(
          '<div class="shift js-edit-shift" data-shift="' +
            data.shiftId +
            '" data-refshift="' +
            data.refShiftId +
            '">'
        );
        html.push(
          '<span class="shift-name" style="background:' +
            background +
            "; color:" +
            color +
            '">' +
            name +
            "</span>"
        );
      }
    }

    html.push('<div class="edit-shift"></div>');

    if (canEditable(data.user.UserId, data.user.SectionId) && name.length) {
      html.push(
        '<i class="icon-copy-single-shift mdi mdi-content-copy js-copy-single-shift"></i>'
      );
      if (data.isDefaultShift === false) {
        html.push(
          '<i class="mdi mdi-close icon-clear-shift js-clear-shift" data-isDefault="' +
            data.isDefaultShift +
            '"></i>'
        );
      }
    }

    html.push("</div>");

    return html.join("");
  }

  Grid.prototype.generateDayBlock = function (user) {
    var html = [];
    var index = 0;

    var momentFromDate = moment(grid.fromDate, "MMM DD, YYYY");
    html.push('<div class="day-block">');
    while (index < 8) {
      var thisDay = momentFromDate.format("YYYY-MM-DD");
      var workingShift = user.WorkingShifts[index];

      var shiftId = null,
        refShiftId = null;

      var isDefaultShift = false;
      if (workingShift) {
        shiftId = workingShift.ShiftId;
        refShiftId = workingShift.RefShiftId;
      } else if (isFutureWeek) {
        var defaultShiftId =
          index === 7 ? user.DefaultShiftIds[0] : user.DefaultShiftIds[index];
        if (defaultShiftId) {
          shiftId = defaultShiftId;
          isDefaultShift = true;
        }
      } else if (index === 7) {
        var defaultShiftId = user.DefaultShiftIds[0];
        if (defaultShiftId) {
          shiftId = defaultShiftId;
          isDefaultShift = true;
        }
      }

      html.push(
        '<div class="day is-default-shift-' +
          isDefaultShift +
          '" data-index="' +
          (index + 1) +
          '"  data-sectionid="' +
          user.SectionId +
          '" data-employeeid="' +
          user.UserId +
          '"  data-date="' +
          thisDay +
          '">'
      );
      var htmlCell = renderOneCell({
        refShiftId: refShiftId,
        user: user,
        thisDay: thisDay,
        shiftId: shiftId,
        isDefaultShift: isDefaultShift,
      });
      html.push(htmlCell);
      html.push("</div>");

      index++;
      momentFromDate.add(1, "days");
    }
    html.push("</div>");
    return html.join("");
  };

  Grid.prototype.generateModifyBy = function (user) {
    var html = [];
    var modifiedDateInMoment = moment(user.ModifiedDate, "YYYY-MM-DD hh:mm:ss");
    modifiedDateInMoment.add(12, "hours");
    html.push('<div class="modify-by">');
    html.push(
      '<div class="staff-name">' + (user.ModifiedBy ? user.ModifiedBy : "")
    );
    html.push(
      '<span class="tooltip-datetime">' +
        modifiedDateInMoment.format("MM/DD/YYYY hh:mm A") +
        " (GMT+8)</span>"
    );
    html.push("</div>");
    if (user.ModifiedBy) {
      html.push('<div class="log-action">');
      html.push('<i class="icon mdi mdi-history icon-view-log"></i>');
      html.push("</div>");
    }
    html.push("</div>");
    return html.join("");
  };

  Grid.prototype.render = function () {
    renderHeader();
    renderBody();
    $(".working-shift-day .tooltip-tipsy").tipsy({ gravity: "sw" });
    $(".working-shift-day .js-edit-shift").workingShift();
    $(".working-shift-day .js-copyFromPreviousWeek").copyWorkingShift();
    $(".working-shift-day .icon-default-shift").editDefaultShift();
    $(".working-shift-day .icon-remark").editShiftRemark();
    $(".working-shift-day .icon-view-log").onViewLog();
    $(".modalViewLog .modal-close").onCloseHistoryLogModal();
    $(".modalViewLog #action-number").onChangeHistoryModalSelection();

    function renderHeader() {
      var html = [];
      html.push('<div class="header">');
      html.push('<div class="staff"><span>Staff</span>');
      if (isShowOrderModeButton()) {
        var checkedProp = workingShiftData.isInOrderMode ? "checked" : "";
        html.push(
          '<div class="order-mode">My Section Mode<label class="switch"><input type="checkbox" class="switch-input" id="order-mode" ' +
            checkedProp +
            '><span class="switch-slider" data-checked="On" data-unchecked="Off"></span></label></div>'
        );
      }
      html.push("</div>");

      var momentFromDate = moment(grid.fromDate, "MMM DD, YYYY");
      var momentToDate = moment(grid.toDate, "MMM DD, YYYY");
      html.push('<div class="day-block">');

      while (momentFromDate <= momentToDate) {
        html.push(
          '<div class="day ' +
            (momentFromDate.isoWeekday() === 6 ||
            momentFromDate.isoWeekday() === 7
              ? "weekend"
              : "") +
            '">' +
            momentFromDate.format("ddd MMM DD") +
            "</div>"
        );
        momentFromDate.add(1, "days");
      }
      html.push("</div>");
      if ($(".body-content").attr("data-isIFrame") === "False") {
        html.push('<div class="modify-by"><span>Modified By</span>');
        html.push("</div>");
      }
      html.push("</div>");
      $(".table-control .working-shift-day").html(html.join(""));
    }

    function renderBody() {
      var html = [];
      if (grid.data.length === 0 || grid.listSectionPublish.length === 0) {
        html.push('<div class="staff-row no-data">There is no data</div>');
      } else {
        var userData = grid.data;

        var listSections = grid.listSectionPublish;
        var sectionUserGroups = [];
        for (var sindex in listSections) {
          var section = listSections[sindex];
          var sectionId = section.SectionId;
          var sectionUsers = [];
          for (var uIndex in userData) {
            var user = userData[uIndex];
            if (section.SectionId === user.SectionId) {
              sectionUsers.push(user);
            }
          }
          if (sectionUsers.length > 0) {
            sectionUserGroups.push({
              SectionPublish: section,
              Users: sectionUsers,
            });
          }
        }

        for (var index in sectionUserGroups) {
          var sectionUserGroup = sectionUserGroups[index];
          var section = sectionUserGroup.SectionPublish;
          var users = sectionUserGroup.Users;
          if (!workingShiftData.isInOrderMode) {
            html.push('<div class="staff-section">');
            html.push(
              '<div class="section-name" data-sectionid="' +
                section.SectionId +
                '">' +
                section.Name +
                "</div>"
            );
            if (
              canEditable(workingShiftData.currentUserId, section.SectionId)
            ) {
              if (section.IsPublished) {
                html.push(
                  '<div class="order-mode tooltip-tipsy" original-title="' +
                    section.DisplayTooltip +
                    '">Publish Schedule<label class="switch"><input checked value="' +
                    section.SectionId +
                    '" type="checkbox" class="switch-input js-publish-mode" data-sectionid="' +
                    section.SectionId +
                    '"><span class="switch-slider" data-checked="On" data-unchecked="Off"></span></label></div>'
                );
              } else {
                html.push(
                  '<div class="order-mode tooltip-tipsy" original-title="' +
                    section.DisplayTooltip +
                    '">Publish Schedule<label class="switch"><input value="' +
                    section.SectionId +
                    '" type="checkbox" class="switch-input js-publish-mode" data-sectionid="' +
                    section.SectionId +
                    '"><span class="switch-slider" data-checked="On" data-unchecked="Off"></span></label></div>'
                );
              }
            }
            html.push("</div>");
          }

          for (var uIndex in users) {
            var user = sectionUserGroup.Users[uIndex];
            html.push(grid.generateUserHtml(user));
          }
        }
      }

      $(".working-shift-day .body").html(html.join(""));
    }
  };

  Grid.prototype.rerenderDayBlock = function (userId) {
    var user;
    for (var i = 0; i < grid.data.length; i++) {
      user = grid.data[i];
      if (user.UserId === userId) break;
    }

    if (user === undefined) return;

    var staffRow = $('.staff-row[data-id="' + userId + '"]');
    if (staffRow.length === 0) return;
    var dayBlock = staffRow.find(".day-block").last();
    dayBlock.html($(grid.generateDayBlock(user)).html());
    dayBlock.find(".tooltip-tipsy").tipsy({ gravity: "sw" });
    dayBlock.find(".js-edit-shift").workingShift();
  };

  function updateWeekControls() {
    $(".prev-week").click(function () {
      var fromDate = $(".weekpicker").data("startDate");
      var date = new Date(fromDate);
      date.setDate(date.getDate() - 7);
      $("#hidden-weekpicker").weekpicker("setDate", date);
      $(".btn-search").click();
    });
    $(".next-week").click(function () {
      var fromDate = $(".weekpicker").data("startDate");
      var date = new Date(fromDate);
      date.setDate(date.getDate() + 7);
      $("#hidden-weekpicker").weekpicker("setDate", date);
      $(".btn-search").click();
    });

    $(".js-icon-quickaccess").click(function (event) {
      event.stopPropagation();
      hideAllPopup();
      workingShiftCore.ExitCopyShiftMode();
      $(".js-quickaccess").toggleClass("is-open");
    });
    $(".working-shift-color").click(function (event) {
      event.stopPropagation();
    });
    $("#employee").on("keypress", function (event) {
      if (event.keyCode === 13) {
        $(".btn-search").trigger("click");
      }
    });
  }

  function hideAllPopupIgnoreDefaultShift() {
    $(".working-shift-color").removeClass("is-open");
    $(".staff-row").removeClass("is-focus");
    $(".edit-shift").removeClass("is-open").empty();
    $(".tipsy").remove();
  }

  function hideAllPopupIgnoreWorkingShiftColor() {
    $(".edit-default-shift").removeClass("is-open").empty();
    $(".staff-row").removeClass("is-focus");
    $(".edit-shift").removeClass("is-open").empty();
    $(".tipsy").remove();
  }

  function hideAllPopup() {
    $(".edit-default-shift").removeClass("is-open").empty();
    $(".working-shift-color").removeClass("is-open");
    $(".staff-row").removeClass("is-focus");
    $(".edit-shift").removeClass("is-open").empty();
    $(".tipsy").remove();
  }

  function handleEventOrderEmployee() {
    $("#order-mode").on("change", function () {
      $("#employee").val("");

      workingShiftCore.ExitCopyShiftMode();
      workingShiftCore.InitCategoryFilter();
      workingShiftCore.InitLocationFilter();
      workingShiftCore.InitExcludeClosedAccountFilter();

      workingShiftData.isInOrderMode = this.checked;
      if (workingShiftData.isInOrderMode) {
        workingShiftCore.DisableWeeklyFilterInOrderMode();

        // auto select current section of User
        var currentSelected =
          workingShiftData.currentSectionId === ""
            ? workingShiftData.sections[0].value
            : workingShiftData.currentSectionId;
        $("#singleSection").val(currentSelected);
        $("#singleSection").material_select();
        $(".btn-search").click();
      } else {
        workingShiftCore.EnableWeeklyFilter();
        workingShiftCore.InitWeeklySectionFilter();
        $(".btn-search").click();
        $("#jssortable").sortable("destroy");
      }
    });

    if (workingShiftData.isInOrderMode) {
      $("#jssortable").sortable({
        handle: ".staff",
        animation: 150,
        ghostClass: "blue-background-class",
      });

      $("#jssortable")
        .off("sortupdate")
        .on("sortupdate", function () {
          var orderedEmployees = $(this).sortable("toArray", {
            attribute: "data-id",
          });

          var data = {
            UserIds: orderedEmployees.toString(),
            SectionId: $("#singleSection").val(),
          };

          workingShiftService.UpdateEmployeeWorkingShiftOrder(
            data,
            function () {}
          );
        });

      $(".js-orderUp")
        .off("click")
        .on("click", function () {
          var current = $(this).closest(".staff-row");
          current.prev().before(current);

          $("#jssortable").trigger("sortupdate");
          $("#jssortable").sortable("refreshPositions");
        });

      $(".js-orderDown")
        .off("click")
        .on("click", function () {
          var current = $(this).closest(".staff-row");
          current.next().after(current);

          $("#jssortable").trigger("sortupdate");
          $("#jssortable").sortable("refreshPositions");
        });

      $(".js-orderTop")
        .off("click")
        .on("click", function () {
          var current = $(this).closest(".staff-row");
          current.prependTo("#jssortable");

          $("#jssortable").trigger("sortupdate");
          $("#jssortable").sortable("refreshPositions");
        });

      $(".js-orderBottom")
        .off("click")
        .on("click", function () {
          var current = $(this).closest(".staff-row");
          current.appendTo("#jssortable");

          $("#jssortable").trigger("sortupdate");
          $("#jssortable").sortable("refreshPositions");
        });
    }
  }

  function handleEventCopyDeleteShift() {
    $(".js-copyWholeShifts").on("click", function () {
      if (!isHost) {
        return;
      }

      workingShiftCore.ExitCopyShiftMode();

      toastr.info("Press ESC to exit Copy shift mode");
      var staffRowElement = $(this).closest(".staff-row");
      var currentPaste = staffRowElement.find(".js-pasteWholeShifts");
      var userId = parseInt(staffRowElement.data("id"));
      var userName = staffRowElement.find(".staff-name").html();
      workingShiftData.copyWholeShifts = {
        FromUserId: userId,
        FromUserName: userName,
      };
      workingShiftData.isInCopyMode = true;
      $(".more-action-function").hide();
      $(".js-pasteWholeShifts")
        .filter(function () {
          return !$(this).is(currentPaste);
        })
        .show();
    });

    $(".js-pasteWholeShifts").on("click", function () {
      //$('.loading-process').css('display', 'inline');
      var staffRowElement = $(this).closest(".staff-row");
      var toUserId = parseInt(staffRowElement.data("id"));
      var model = {
        FromUserId: workingShiftData.copyWholeShifts.FromUserId,
        ToUserId: toUserId,
        FromDate: grid.fromDate,
        ToDate: moment(grid.toDate).add(-1, "days").format("MMM DD, YYYY"),
      };

      workingShiftService.CopyWholeShifts(model, function (result) {
        updateWorkingShiftDataRow(toUserId, result, false);
        grid.rerenderDayBlock(toUserId);
        staffRowElement.addClass("is-focus");
        //$('.loading-process').css('display', 'none');
      });
    });

    $(".js-deleteWholeShifts").on("click", function () {
      if (!isHost) {
        return;
      }
      let toDate = moment(grid.toDate).add(-1, "days").format("MMM DD, YYYY");
      workingShiftCore.ExitCopyShiftMode();
      var staffRowElement = $(this).closest(".staff-row");
      var userId = parseInt(staffRowElement.data("id"));
      var userName = staffRowElement.find(".staff-name").html();
      var messageConfirmText =
        "Do you want to delete " +
        userName +
        "'s shifts <br />from " +
        grid.fromDate +
        " to " +
        toDate +
        "?";
      Swal.fire({
        html: messageConfirmText,
        type: "question",
        showCancelButton: true,
        reverseButtons: true,
        animation: false,
        heightAuto: false,
        scrollbarPadding: false,
      }).then((result) => {
        if (result.value) {
          // $('.loading-process').css('display', 'inline');

          var model = {
            UserId: userId,
            FromDate: grid.fromDate,
            ToDate: toDate,
          };

          workingShiftService.DeleteWholeShifts(model, function () {
            removeWorkingShiftDataRow(userId);
            grid.rerenderDayBlock(userId);
            staffRowElement.addClass("is-focus");
            // $('.loading-process').css('display', 'none');
          });
        }
      });
    });
  }

  function handleEventExitCopyShiftMode() {
    $(document).keydown(function (e) {
      if (
        e.keyCode == 27 &&
        (workingShiftData.isInCopyMode === true ||
          workingShiftData.isInCopyModeSingle === true)
      ) {
        workingShiftCore.ExitCopyShiftMode();
        hideAllPopup();
      }
    });
  }

  function handlePublishWeeklyWorkingShift() {
    $(document).ready(function () {
      $(".js-publish-mode")
        .off()
        .on("change", function (event) {
          event.preventDefault();
          event.stopPropagation();
          var ele = $(this);
          var sectionId = $(this).val();
          var dtPicker = $(".weekpicker").val();
          var dtArray = dtPicker.split("-");
          var fromDate = dtArray[0].trim();
          var toDate = dtArray[1].trim();
          var isPublished = this.checked;

          var model = {
            SectionId: sectionId,
            FromDate: moment(fromDate).add(1, "days").format("MMM DD, YYYY"),
            ToDate: toDate,
            IsPublished: isPublished,
          };

          if (model.SectionId && model.FromDate && model.ToDate) {
            $(".loading-process").css("display", "inline");
            workingShiftService
              .PublishWeeklyWorkingShift(model, function (res) {
                if (res.result.IsSuccess) {
                  var data = res.result.Data;
                  var groups = res.groupByUser;
                  if (data.IsPublished === true) {
                    ele.parent().parent().addClass("tooltip-tipsy");
                    ele
                      .parent()
                      .parent()
                      .attr("original-title", data.DisplayTooltip);
                    for (var i = 0; i < groups.length; i++) {
                      updateWorkingShiftDataRow(
                        groups[i].UserId,
                        groups[i].WorkingShift,
                        true
                      );
                      grid.rerenderDayBlock(groups[i].UserId);
                    }
                  } else {
                    ele.parent().parent().removeClass("tooltip-tipsy");
                    ele.parent().parent().attr("original-title", "");
                  }
                }
                $(".loading-process").css("display", "none");
              })
              .error(function (error) {
                var errorMessage =
                  "An error occurred when pasting, please try again.";
                Swal.fire({
                  html: errorMessage,
                  type: "error",
                  animation: false,
                });
                $(".loading-process").css("display", "none");
                Raven.captureMessage(JSON.stringify(error));
              });
          }
        });
    });
  }
  return {
    init: function () {
      $("main").click(function (event) {
        hideAllPopup();
      });

      $("body").on("focus", ".materialize-textarea", function () {
        $(this).trigger("autoresize");
      });

      if (!isHostShiftColor) {
        $("#btn-update-color").hide();
        $("#bg-color").prop("disabled", true);
        $("#text-color").prop("disabled", true);
        $(".format-style").remove();
        $(".addnew_shift").remove();
      }

      $("#dropdownTimezone > li").click(function () {
        var settingId = parseInt($(this).data("setting-id"));
        var name = $(this).data("name");
        var model = {
          SettingId: settingId,
        };

        workingShiftService.UpdateTimezone(model, function () {
          location.reload();
        });
      });
      $("#dropdownTimezoneIFrame > li").click(function () {
        var offset = parseInt($(this).data("offset"));
        var defaultGMT8 = 8;
        var defaultMinus = 60;
        var getGMT = defaultGMT8 - -offset / defaultMinus;
        var url = common.ResolveRootUrl(
          "WeeklyWorkingShift/Iframe?gmt=" + getGMT
        );
        window.location.href = url;
      });

      updateWeekControls();
      handleEventExitCopyShiftMode();
    },
    render: function (data) {
      var fromDate = $(".weekpicker").data("startDate");
        var toDate = moment($(".weekpicker").data("endDate"), "MMM DD, YYYY")
        .add(1, "days")
        .format("MMM DD, YYYY");
      $(".txt-week").text(fromDate + " - " + toDate);
      $(".table-daily").hide();
      $("#total-weekly").text(data.WorkingShiftUsers.length);
      $("#result").html(data);

      workingShiftUsers = data.WorkingShiftUsers;
      var listSectionPublish = data.ListSectionPublish;
      workingShiftSetting = new WorkingShiftSetting(data.WorkingShiftColors);
        workingShiftSetting.render();
        workingShiftSetting.initEvents();
      
      workingShiftSetting.handleUpdateColor();
      var momentToDate = moment(toDate, "MMM DD, YYYY");
      var today = moment().startOf("day");
      isFutureWeek = momentToDate > today.add(7, "day");
      grid = new Grid(fromDate, toDate, workingShiftUsers, listSectionPublish);
      grid.render();
      $(".dropdown-button").dropdown({
        belowOrigin: true,
        constrain_width: false,
      });
      handleEventOrderEmployee();
      handleEventCopyDeleteShift();
      handlePublishWeeklyWorkingShift();
    },
    findShiftById: function (shiftId) {
      return findShiftById(shiftId);
    },
  };
})(jQuery);
