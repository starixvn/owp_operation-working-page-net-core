"use strict";

var dailyWorkingShift = (function ($) {
    var $shifts = [];
    var BET_TYPE_1H = [7, 8, 15];
    var BET_TYPE_GROUPS = {
        'HDP': [1],
        'OU': [3],
        '1X2': [5],
        '1H': [7, 8, 15],
        '15mins': [-1],
        '10mins': [-2],
        'CR': [-3, -4],
        'CR15': [-5],
        'TB': [-6, -7],
        'TB15': [-8],
        'LA': [-1002],
        'FM': [-88],
        'AIO': [-89],
        'Others': []
    };
    var groupBetType_1X2_FM_AIO = ["AIO", "FM", "1X2"];
    var groupBetType_Others = ["Others", "15mins", "10mins", "CR", "CR15", "TB", "TB15", "LA"];
    var today = moment().format('MM/DD/YYYY');
    var queryDate = common.urlParam('date');
    var currentDate = queryDate || today;
    var mondayofCurrentWeek = moment(currentDate, 'MM/DD/YYYY').startOf('isoweek');
    var fourlatestWeeks = moment(mondayofCurrentWeek.subtract(28, "days"));
    function getBetTypeName(betTypeGroupName) {
        switch (betTypeGroupName) {
            case 'CR15':
                return "CR+15'";
            case 'TB15':
                return "TB+15'";
            default:
                return betTypeGroupName;
        }
    }

    function Match(matchData, betTypes, isETPK) {
        this.matchId = matchData.OWPMatchId;
        this.matchNo = matchData.MatchNo;
        this.maxBet = matchData.MaxBet;
        this.groupId = matchData.CategoryId;
        this.eventDateTime = moment(matchData.EventDateTime, "YYYY-MM-DDTHH:mm:ss").format();
        this.homeName = matchData.HomeName;
        this.leagueName = matchData.LeagueName;
        this.otherProvider = matchData.OtherProvider;
        this.provider = matchData.Provider;
        this.awayName = matchData.AwayName;
        this.betTypes = betTypes;
        this.overlap = 0;
        this.isAllowedToViewDetail = matchData.IsAllowedToViewDetail;
        this.isETPK = isETPK;

        this.cssClassNameHighlight = function () {
            var found_1X2_FM_AIO = false;
            var found_others = false;
            var className = '';
            var betTypeGroups = this.getBetTypeGroups();

            for (var i = 0; i < betTypeGroups.length; i++) {
                if (groupBetType_1X2_FM_AIO.indexOf(betTypeGroups[i].groupName) !== -1) {
                    found_1X2_FM_AIO = true;
                    break;
                }
            }

            for (i = 0; i < betTypeGroups.length; i++) {
                if (groupBetType_Others.indexOf(betTypeGroups[i].groupName) !== -1) {
                    found_others = true;
                    break;
                }
            }

            if (!found_1X2_FM_AIO && !found_others) {
                className = '';
            } else if (found_1X2_FM_AIO && !found_others) {
                className = 'minor-match-fm-aio';
            } else if (found_others) {
                className = 'minor-match';
            }

            return className;
        };

        this.isFullTime = function () {
            for (var i = 0; i < this.betTypes.length; i++) {
                if (this.isETPK) {
                    return false;
                }
                if (BET_TYPE_1H.indexOf(this.betTypes[i]) === -1) {
                    return true;
                }
            }

            return false;
        };

        this.getBetTypeGroups = function () {
            var groups = [];
            var hasOthers = false;
            this.betTypes.forEach(function (betType) {
                var hasGroup = false;
                for (var groupName in BET_TYPE_GROUPS) {
                    var groupBetTypes = BET_TYPE_GROUPS[groupName];
                    if (groupBetTypes.indexOf(betType) !== -1) {
                        if (groups.indexOf(groupName) === -1) {
                            groups.push({ etpkAssignmentNote: isETPK ? 'ET/PK' : null, groupName: groupName });
                        }

                        hasGroup = true;
                        break;
                    }
                }

                if (!hasGroup) {
                    hasOthers = true;
                }
            });

            if (hasOthers) {
                groups.push({ etpkAssignmentNote: isETPK ? 'ET/PK' : null, groupName: 'Others' });
            }
            return groups;
        };

        this.getBox = function () {
            var selectDate = moment($('.txt-day').text());
            var date = moment(this.eventDateTime);
            var isFullTime = this.isFullTime();
            var totalMinute = isFullTime ? 120 : 60;
            var endDate = moment(this.eventDateTime).add(totalMinute, 'minutes');
            var startMinute = ((date.hour() + 12) % 24) * 60 + date.minute();

            if (this.isETPK) {
                date = moment(this.eventDateTime).add(120, 'minutes');
                totalMinute = 60;
                endDate = moment(this.eventDateTime).add(120 + totalMinute, 'minutes');
                startMinute = (((date.hour() + 12) % 24) * 60 + date.minute()) % (24 * 60);
            }
            if (selectDate.date() === endDate.date()) {
                if (endDate.hour() < 12) {
                    return null;
                } else if (date.hour() < 12) {
                    startMinute = 0;
                    totalMinute = (endDate.hour() - 12) * 60 + date.minute();
                }
            } else {
                if (endDate.hour() > 11) {
                    totalMinute = (12 - date.hour()) * 60 - date.minute();
                }
            }

            return {
                startMinute: startMinute,
                totalMinute: totalMinute
            };
        };

        this.generateHtml = function (large) {
            var box = this.getBox();
            if (box === null) {
                return '';
            }

            var date = moment(this.eventDateTime);
            var htmlMatch = [];

            var betTypeGroups = this.getBetTypeGroups();

            var cssClassNameHighlight = this.cssClassNameHighlight();

            htmlMatch.push('<div class="' + cssClassNameHighlight + '  match-box group-' + this.groupId + ' ' + (large ? 'large' : '') + ' ' + '" data-overlap="' + this.overlap + '" data-start-minute="' + box.startMinute + '" data-total-minute="' + box.totalMinute + '" data-match-id="' + this.matchId + '" data-is-etpk="' + this.isETPK + '">');
            htmlMatch.push('<p class="match-name">');
            if (this.maxBet > 0) {
                var maxBetText = this.maxBet;
                if (this.maxBet >= 1000) {
                    maxBetText = this.maxBet / 1000 + 'K';
                }
                htmlMatch.push('<span class="maxbet">' + maxBetText + '</span> - ');
            }

            if (this.provider) {
                htmlMatch.push('<span class="runner"> ' + this.provider + ' </span> - ');
            }

            htmlMatch.push('<span class="match-league">' + this.leagueName + '</span>');

            if (large) {
                htmlMatch.push('<p class="match-home-away">' + (this.matchNo ? '#' + this.matchNo : '') + ' ' + this.homeName + ' -vs- ' + this.awayName + '</p>');
                htmlMatch.push('<span class="close"></span>');

                if (common.hasDetailInformationPermission() && (isFullViewPermisson === 'True' || this.isAllowedToViewDetail)) {
                    htmlMatch.push('<a href="javascript:;" match-id="' + this.matchId + '" class="link-livedetail"><i class="mdi mdi-arrow-right"></i></a>');
                }
            }
            htmlMatch.push('</p>');

            var displayTime = large || date.minute() !== 0 ? date.format('hh:mm A') : '';

            htmlMatch.push('<p class="match-time">' + displayTime + '</p>');
            if (large && this.otherProvider) {
                htmlMatch.push('<p class="provider">Other Provider: ' + this.otherProvider + '</p>');
            }

            htmlMatch.push('<p class="betType-group">');
            var shownBetTypeTotal = betTypeGroups.length;

            if (!large) {
                shownBetTypeTotal = Math.min(2, betTypeGroups.length);
            }
            for (var i = 0; i < shownBetTypeTotal; i++) {
                htmlMatch.push('<span class="betType-item">');
                htmlMatch.push('<span class="betType-color betType-' + betTypeGroups[i].groupName + '">');
                htmlMatch.push('</span>');
                if (large) {
                    var betTypeName = getBetTypeName(betTypeGroups[i].groupName);
                    htmlMatch.push(betTypeGroups[i].etpkAssignmentNote !== null ? `${betTypeGroups[i].etpkAssignmentNote} (${betTypeName})` : betTypeName);
                }
                htmlMatch.push('</span>');
            }
            if (!large && shownBetTypeTotal < betTypeGroups.length) {
                htmlMatch.push('<span class="betType-color more">' + (betTypeGroups.length - shownBetTypeTotal) + '+</span>');
            }
            htmlMatch.push('</p>');
            htmlMatch.push('</div>');
            return htmlMatch.join('');
        };
    }

    function User(userData, timeSheet) {
        this.matches = [];
        this.name = userData.UserName;
        this.shiftId = 0;
        this.shiftCode = '';
        var shift = findShiftById(userData.ShiftId);
        if (shift) {
            this.shiftId = shift.ShiftId;
            this.shiftCode = shift.ShiftCode;
        }

        this.section = userData.SectionName;
        this.levelName = userData.LevelName;
        this.groupName = userData.GroupName;
        this.userId = userData.UserId;
        this.remark = userData.Remark;
        this.timeSheet = timeSheet;
        this.notes = userData.Notes;

        this.addMatch = function (match) {
            var box = match.getBox();
            if (box === null) {
                return;
            }

            var index;
            for (index = 0; index < this.matches.length; index++) {
                if (moment(match.eventDateTime) < moment(this.matches[index].eventDateTime)) {
                    break;
                }
            }

            this.matches.splice(index, 0, match);
        };

        this.findMatch = function (matchId, isETPK) {
            for (var i = 0; i < this.matches.length; i++) {
                var match = this.matches[i];
                if (match.matchId === matchId && match.isETPK === isETPK) {
                    return match;
                }
            }
        };

        this.checkMatchOverlap = function () {
            if (this.matches.length < 2) {
                return;
            }

            var box = this.matches[0].getBox();
            var overlap = 0;
            var boxMatchId = this.matches[0].matchId;
            var boxId = 0;

            for (let i = 1; i < this.matches.length; i++) {
                var match = this.matches[i];
                if (!match.isETPK) {
                    var b = match.getBox();

                    if (this.matches[boxId].isETPK) {
                        box.startMinute = box.startMinute - 120;
                        box.totalMinute = 180;
                    }

                    if (this.matches[boxId + 1] !== undefined && this.matches[boxId + 1].matchId === boxMatchId && this.matches[boxId + 1].isETPK) {
                        box.totalMinute = 180;
                    }

                    if (b.startMinute >= box.startMinute && b.startMinute < box.startMinute + box.totalMinute) {
                        overlap++;
                    } else {
                        overlap = 0;
                    }

                    this.matches[i].overlap = overlap;
                    box = b;
                    boxMatchId = this.matches[i].matchId;
                    boxId = i;
                }
            }

            for (let j = 0; j < this.matches.length; j++) {
                if (!this.matches[j].isETPK) {
                    if (this.matches[j + 1] !== undefined && this.matches[j + 1].matchId === this.matches[j].matchId && this.matches[j + 1].isETPK) {
                        this.matches[j + 1].overlap = this.matches[j].overlap;
                    }
                }
            }
        };

        this.generateHtml = function () {
            var userRemark = this.remark === null ? '' : common.htmlEntities(this.remark);
            var remarkIconClass = "icon-remark mdi mdi-message-text tooltip-tipsy";
            if (userRemark) {
                remarkIconClass += ' has-remark';
            }

            var isHostDaily = $('.body-content').attr('data-isHostDaily') === 'True';

            var html = [];
            html.push('<div class="staff-row" data-id="' + this.userId + '">');
            html.push('<div class="staff"><div class="avatar light-blue"><i class="mdi mdi-account"></i></div><div class="staff-info">');
            html.push('<p><span class="staff-name txt-bold">' + this.name + '</span></p><p><span class="staff-team">' + this.section + '</span></p>');
            html.push('<p class="remark-container"><i class="' + remarkIconClass + '" original-title="Remark"></i><span class="show-remark tooltip-tipsy" original-title="' + userRemark + '">' + userRemark + '</span></p>');
            html.push('<div class="remark-popover">');
            html.push('<div class="input-field"><div class="label">Remark</div><textarea class="materialize-textarea" maxlength="200"></textarea></div>');
            html.push('<i class="icon-close-popover mdi mdi-close"></i>');
            html.push('<div class="btn-group"><a class="waves-effect waves-light btn btn-cancel">Cancel</a><a class="waves-effect waves-light btn btn-save" data-id="' + this.userId + '">Save</a></div>');
            html.push('</div>');
            html.push('</div></div>');
            html.push('<div class="group">' + (this.groupName ? this.groupName : '') + '</div>');
            html.push('<div class="level">' + (this.levelName ? this.levelName : '') + '</div>');
            var noteIconClass = "note";
            if (this.notes.length > 0) {
                noteIconClass += ' has-note';
            }
            html.push('<div class="' + noteIconClass + '"><span class="mdi mdi-comment"></span>');
            html.push('<div class="note-popover">');
            html.push('<span class="icon-close-popover mdi mdi-close"></span>');
            if (isHostDaily) {
                html.push('<form class="create-note"><input class="note-textinput" type="text" maxlength="4000" placeholder="Write a note"></form>');
            }
            if (this.notes.length > 0) {
                html.push('<ul class="note-list">');
                for (var i = 0; i < this.notes.length; i++) {
                    var note = this.notes[i];
                    html.push('<li class="note-item" data-id="' + note.EmployeeWorkingShiftNoteId + '">');
                    html.push('<div class="content">');
                    html.push('<div class="text-left"><strong class="user-name">@' + note.Username + '</strong>' + '<span class="user-note">' + ' ' + (note.Note === null ? '' : common.htmlEntities(note.Note)) + '</span></div>');
                    var displayLastUpdated = '';
                    if (note.LastUpdated) {
                        displayLastUpdated = moment(note.LastUpdated, 'YYYY-MM-DDTHH:mm:ss').format('MM/DD/YYYY hh:mm A');
                    }
                    html.push('<span class="date-time">' + displayLastUpdated + '</span>');
                    if (isHostDaily) {
                        html.push('<div class="icon-button">');
                        html.push('<button class="icon-edit mdi mdi-pencil"></button>');
                        html.push('<button class="icon-delete mdi mdi-delete"></button>');
                        html.push('</div>');
                    }
                    html.push('</div>');
                    html.push('</li>');
                }
                html.push('</ul>');
            } else {
                html.push('<div class="no-data">There is no data</div>');
            }
            html.push('</div>');
            html.push('</div>');
            html.push('<div class="shift"><div class="shift-time" data-id="' + this.shiftId + '">' + this.shiftCode + '</div></div>');

            // draw timesheet
            html.push('<div class="time-block">');
            for (i = 0; i < 24; i++) {
                if (this.timeSheet[i] === undefined) {
                    html.push('<div class="time"></div>');
                } else {
                    html.push('<div class="time"><div class="working" data-scale="' + this.timeSheet[i] + '"></div></div>');
                }
            }

            // draw matches
            for (var j = 0; j < this.matches.length; j++) {
                var box = this.matches[j].generateHtml(false); // Tyler
                html.push(box);
            }

            html.push('</div>');
            html.push('</div>');
            return html.join('');
        };
    }

    function TimeRange(startTime, endTime) {
        this.startTime = startTime;
        this.endTime = endTime;
        this.update = function (timeSheet) {
            var startId = (this.startTime.hours() + 12) % 24;
            var endId = (this.endTime.hours() + 12) % 24;
            if (this.startTime.Minutes !== 0) {
                timeSheet[startId] = -this.startTime.minutes() / 60;
                startId++;
            }
            if (this.endTime.minutes() === 0 && endId === 0) {
                endId = 24;
            }

            for (var i = startId; i < endId; i++) {
                timeSheet[i] = 0;
            }

            if (this.endTime.minutes() !== 0) {
                timeSheet[endId] = this.endTime.minutes() / 60;
            }
        };
    }
    TimeRange.parse = function (shift, date) {
        var startTime = moment(shift.ShiftStartTime);
        var endTime = moment(shift.ShiftEndTime);
        var fromTime = moment(date).add(12, 'hours');
        var toTime = moment(date).add(36, 'hours');

        if (startTime < fromTime) {
            startTime = fromTime;
        }

        if (endTime > toTime) {
            endTime = toTime;
        }

        if (startTime.isSame(endTime)) {
            return undefined;
        }

        return new TimeRange(startTime, endTime);
    };

    function findShiftById(shiftId) {
        for (var i = 0; i < $shifts.length; i++) {
            if ($shifts[i].ShiftId === shiftId) {
                return $shifts[i];
            }
        }
    };

    function assignmentStatistics(assignmentStatisticsData) {
        renderTable();

        function renderTable() {
            var table = $(".js-assigment-statistics");
            table.empty();
            var totalType = $(".type-row");
            totalType.each(it => {
                if (totalType[it].checked) {
                    var row = renderRow(totalType[it].name);
                    table.append(row);
                }
            });
            $(".table-body").css({ 'margin-top': $(".sticky-filter").outerHeight() + 25 });
        }

        function renderRow(name) {
            var row = [];
            switch (name) {
                case "Match Assigned":
                    row = createRow(assignmentStatisticsData.MatchAssigned, "Match Assigned").join("");
                    break;
                case "Trader Assigned":
                    row = createRow(assignmentStatisticsData.TraderAssigned, "Trader Assigned").join("");
                    break;
                case "LA Assigned":
                    row = createRow(assignmentStatisticsData.LAAssigned, "LA Assigned").join("");
                    break;
                case "Match Selected":
                    row = createRow(assignmentStatisticsData.MatchSelected, "Match Selected").join("");
                    break;
                default:
                    row = null;
            }
            return row;
        }

        function createRow(dataCell, nameRow) {
            var row = [];
            row.push("<div class='js-box-" +
                nameRow.toLowerCase().replace(/\s/g, "-") +
                "'>");
            row.push("<div class='statistics-match'>");
            row.push("<div class='section-label'>");
            row.push("<div class='label'>" + nameRow + "</div>");
            row.push("</div>");
            row.push(" <div class='section-value'>");
            dataCell.forEach(dt => row.push("<div class='time-statistics'>" + dt + "</div>"));
            row.push("</div>");
            row.push("</div>");
            return row;
        }
    };

    function getAssignmentStatisticsRowState(username) {
        if (username) return JSON.parse(localStorage.getItem(username + "-DWS"));
        return null;
    }
    function storeAssignmentStatisticsRowState(username, assignmentStatisticsRowsState) {
        if (Object.keys(assignmentStatisticsRowsState).length === 4)
            localStorage.setItem(username + "-DWS", JSON.stringify(assignmentStatisticsRowsState));
    }

    function generateAssignmentStatisticsDropdown() {
        var html = [];
        generateAssignmentStatistics();

        function generateAssignmentStatistics() {
            var stateFillter = getRowsStateFillter();

            html.push('<div class="assignment-statistics-type-table">');
            html.push(
                "<input type='checkbox' class='type-row filled-in checkbox' name='Match Assigned' id='MatchAssigned'" +
                stateFillter["Match Assigned"] + ">");
            html.push("<label for='MatchAssigned'>Match Assigned</label>");
            html.push(
                "<input type='checkbox' class='type-row filled-in checkbox' name='Trader Assigned' id='TraderAssigned'" +
                stateFillter["Trader Assigned"] + ">");
            html.push("<label for='TraderAssigned'>Trader Assigned</label>");

            html.push("<input type='checkbox' class='type-row filled-in checkbox' name='LA Assigned' id='LAAssigned'" +
                stateFillter["LA Assigned"] + ">");
            html.push("<label for='LAAssigned'>LA Assigned</label>");

            html.push(
                "<input type='checkbox' class='type-row filled-in checkbox' name='Match Selected' id='MatchSelected'" +
                stateFillter["Match Selected"] + ">");
            html.push("<label for='MatchSelected'>Match Selected</label>");
            html.push("</div>");
            $(".assignment-statistics-type-config").after(html.join(""));
        }

        function getRowsStateFillter() {
            var state = getAssignmentStatisticsRowState(username);
            if (!state) {
                var assignmentStatisticsRowsState = {
                    'Match Assigned': "checked",
                    'Trader Assigned': "checked",
                    'LA Assigned': "checked",
                    'Match Selected': "checked"
                };
                storeAssignmentStatisticsRowState(username, assignmentStatisticsRowsState);
                state = getAssignmentStatisticsRowState(username);
            }
            return state;
        }
    }
    return {
        lastModel: null,
        updateStickyWidth: function () {
            $('#sticky-filter').width($('.table-body').width() + 2);
        },
        init: function () {
            function generateColorTable() {
                var html = [];
                html.push('<div class="color-table">');

                html.push('<div class="color-group league-group">');
                html.push('<div class="title">League Group</div>');
                for (var i = 1; i <= 4; i++) {
                    html.push('<div class="color"><span class="group-' + i + '"></span>M' + i + '</div>');
                }
                html.push('</div>');

                html.push('<div class="color-group bettype-group">');
                html.push('<div class="title">Bettype</div>');
                for (var groupName in BET_TYPE_GROUPS) {
                    var betTypeName = getBetTypeName(groupName);
                    html.push('<div class="color"><span class="betType-' + groupName + '"></span>' + betTypeName + '</div>');
                }
                html.push('</div>');

                html.push("</div>");
                $('.daily-option-config').after(html.join(''));
            }

            generateColorTable();

            generateAssignmentStatisticsDropdown();

            $(".prev-day").on("click",
                function() {
                    var date = new Date($(".txt-day").text());
                    var isValidTimeRangeQuery = moment(date).diff(fourlatestWeeks, "days") > 0;
                    if (isValidTimeRangeQuery) {
                        date.setDate(date.getDate() - 1);
                        $(".datepicker").val(moment(date).format("MM/DD/YYYY"));
                        $(".btn-search").click();
                    }
                    
                });
            $('.next-day').on('click', function () {
                var date = new Date($('.txt-day').text());
                date.setDate(date.getDate() + 1);
                $('.datepicker').val(moment(date,'MM/DD/YYYY'));
                $(".btn-search").click();
            });
            $('.daily-option-config').click(function () {
                $('.color-table').toggle();
            });
            $('.assignment-statistics-type-config').click(function () {
                $('.assignment-statistics-type-table').toggle();
            });
        },
        getShiftsAndSearch: function () {
            dailyWorkingShiftService.getShifts(function (shifts) {
                $shifts = shifts;
                var shiftCodes = $.map(shifts, function (shift) {
                    
                    return {
                        text: shift.ShiftCode,
                        value: shift.ShiftId,
                        enable: true,
                        checked: true
                    };
                });
                $('#shift').fanexMultiSelect({
                    minWidth: 120,
                    listData: shiftCodes,
                    showFilter: true,
                    customCheckAllText: 'All',
                    useCustomCheckAllText: true,
                    noneSelectedText: 'Select shifts'
                });
                $(".btn-search").click();
            });
        },
        onClickUserNote: function () {
            $(".working-shift-hour .note").on("click", function (event) {
                $('.note-popover').removeClass('is-open');
                event.stopPropagation();

                var _this = this;
                var notePopover = $(_this).find('.note-popover');
                notePopover.toggleClass('is-open');

                var editButton = notePopover.find('.icon-edit');
                editButton.off('change');
                editButton.on('click', function () {
                });
            });
        },
        onClickCloseUserNote: function () {
            $(".working-shift-hour .note-popover .mdi-close").on("click", function (event) {
                $('.note-popover').removeClass('is-open');
                event.stopPropagation();
            });
        },
        onInsertUserNote: function () {
            $(".working-shift-hour .note-popover .note-textinput").on("keypress", function (event) {
                var _this = this;
                var keyCode = event.keyCode;
                if (keyCode === 13) {
                    event.preventDefault();
                    if ($.trim($(_this).val()) != '') {
                        var staffRow = $(_this).closest('.staff-row');
                        var noteList = staffRow.find('.note-list');
                        var userId = parseInt(staffRow.data('id'));
                        var date = new Date($('.txt-day').text());
                        date.setDate(date.getDate() + 1);

                        var insertModel = {
                            ApiKey: 'employee-working-shift-insert-note',
                            UserId: userId,
                            ShiftDate: date,
                            Note: $(_this).val()
                        };
                        dailyWorkingShiftService.insertUpdateQueryEmployeeWorkingShiftNote(insertModel, function (insertResult) {
                            var queryModel = {
                                ApiKey: 'employee-working-shift-query-note-by-id',
                                EmployeeWorkingShiftNoteId: insertResult[0].NoteId
                            };
                            dailyWorkingShiftService.insertUpdateQueryEmployeeWorkingShiftNote(queryModel, function (queryResult) {
                                var displayLastUpdated = '';
                                if (queryResult[0].LastUpdated) {
                                    displayLastUpdated = moment(queryResult[0].LastUpdated, 'YYYY-MM-DDTHH:mm:ss').format('MM/DD/YYYY hh:mm A');
                                }
                                if (noteList.length > 0) {
                                    let noteItem = '<li class="note-item" data-id="' + queryResult[0].EmployeeWorkingShiftNoteId + '"><div class="content"><div class="text-left"><strong class="user-name">@' + queryResult[0].UserName + '</strong>' + '<span class="user-note">' + ' ' + (queryResult[0].Note === null ? '' : common.htmlEntities(queryResult[0].Note)) + '</div><span class="date-time">' + displayLastUpdated + '</span><div class="icon-button"><button class="icon-edit mdi mdi-pencil"></button><button class="icon-delete mdi mdi-delete"></button></div></li>';
                                    noteList.prepend(noteItem);
                                    $(_this).val('');
                                }
                                else {
                                    var noDataElement = staffRow.find('.no-data');
                                    noDataElement.remove();
                                    let noteItem = '<ul class="note-list"><li class="note-item" data-id="' + queryResult[0].EmployeeWorkingShiftNoteId + '"><div class="content"><div class="text-left"><strong class="user-name">@' + queryResult[0].UserName + '</strong>' + '<span class="user-note">' + ' ' + (queryResult[0].Note === null ? '' : common.htmlEntities(queryResult[0].Note)) + '</div><span class="date-time">' + displayLastUpdated + '</span><div class="icon-button"><button class="icon-edit mdi mdi-pencil"></button><button class="icon-delete mdi mdi-delete"></button></div></li></ul>';
                                    var notePopover = staffRow.find('.note-popover');
                                    notePopover.append(noteItem);
                                    $(_this).val('');
                                    staffRow.find('.note').addClass('has-note');
                                }
                                $('.working-shift-hour .note-popover .note-item .icon-button .icon-delete').deleteUserNote();
                                $('.working-shift-hour .note-popover .note-item .icon-button .icon-edit').editUserNote();
                            });
                        }).error(function () {
                            toastr.error('Something went wrong');
                        });
                    }
                }
            });
        },
        render: function (data, date) {
            function findMatch(matchId) {
                for (var x = 0; x < data.Matches.length; x++) {
                    var matchData = data.Matches[x];
                    if (matchData.OWPMatchId === matchId) {
                        return matchData;
                    }
                }
            }

            var users = [];

            for (var i = 0; i < data.Users.length; i++) {
                var userData = data.Users[i];
                var timeSheet = {};
                for (var j = 0; j < userData.Shifts.length; j++) {
                    var shift = userData.Shifts[j];
                    var formattedShift = moment(shift, 'YYYY-MM-DDTHH:mm:ssZ');
                    var formattedDate = moment(date, 'YYYY-MM-DD');
                    var timeRange = TimeRange.parse(formattedShift, formattedDate);
                    if (timeRange) {
                        timeRange.update(timeSheet);
                    }
                }

                var user = new User(userData, timeSheet);
                if (userData.Matches) {
                    for (var k = 0; k < userData.Matches.length; k++) {
                        var userMatch = userData.Matches[k];
                        var matchData = findMatch(userMatch.OWPMatchId);
                        if (matchData) {
                            var match = new Match(matchData, userMatch.BetTypes, userMatch.isETPK);
                            user.addMatch(match);
                        }
                    }
                }

                user.checkMatchOverlap();
                users.push(user);
            }

            $('#total-daily').text(data.Users.length);
            var tableBody = $('.working-shift-hour .body');
            tableBody.html('');

            if (users.length === 0) {
                tableBody.append('<div class="staff-row no-data">There is no data</div');
            } else {
                for (i = 0; i < users.length; i++) {
                    user = users[i];
                    var row = user.generateHtml();
                    tableBody.append(row);
                }

                // transform timesheet
                tableBody.find('.time>.working').each(function (index, element) {
                    var scale = $(element).data('scale');
                    if (scale > 0) {
                        $(element).css({
                            'width': scale * 100 + '%'
                        });
                    } else if (scale < 0) {
                        $(element).css({
                            'width': (1 + scale) * 100 + '%',
                            'margin-left': -scale * 100 + '%'
                        });
                    }
                });

                // transform match
                tableBody.find('.staff-row .time-block').each(function () {
                    var maxOverlap = 0;
                    $(this).find('.match-box').each(function () {
                        var startMinute = $(this).data('start-minute');
                        var totalMinute = $(this).data('total-minute');
                        $(this).css({
                            'left': startMinute * 100 / (60 * 24) + '%',
                            'width': totalMinute * 100 / (60 * 24) + '%'
                        });

                        var overlap = $(this).data('overlap');
                        maxOverlap = Math.max(maxOverlap, overlap);
                        if (overlap !== 0) {
                            $(this).css({
                                'top': overlap * 16 + 'px',
                                'height': 26 + 'px'
                            });
                        }
                    }).click(function () {
                        $('.match-box.large').remove();

                        var row = $(this).closest('.staff-row');
                        var userName = row.find('.staff-name').text();
                        var matchId = $(this).data('match-id');
                        var isETPK = $(this).data('is-etpk');
                        for (var i = 0; i < users.length; i++) {
                            if (users[i].name === userName) {
                                var match = users[i].findMatch(matchId, isETPK);
                                if (match) {
                                    var html = match.generateHtml(true);
                                    var largeElement = $(html);
                                    $(this).after(largeElement);
                                    if ($(this).offset().left + largeElement.outerWidth() > $('.body-content').outerWidth()) {
                                        largeElement.css({
                                            'right': 0
                                        });
                                    } else {
                                        largeElement.css({
                                            'left': $(this).css('left')
                                        });
                                    }
                                    largeElement.find('.close').click(function () {
                                        largeElement.remove();
                                    });
                                    largeElement.find('.link-livedetail').click(function () {
                                        var matchId = $(this).attr('match-id');
                                        common.modalLiveDetail(matchId, function () {
                                            var dailyModel = dailyWorkingShift.lastModel;
                                            dailyWorkingShiftService.search(dailyModel, function (result) {
                                                $('.loading-process').css('display', 'none');
                                                $('#result').html('');
                                                $('.txt-day').text(dailyModel.date);
                                                dailyWorkingShift.render(result, dailyModel.date);
                                                dailyWorkingShift.updateStickyWidth();
                                            }).error(function () {
                                                $('.loading-process').css('display', 'none');
                                            });
                                        });
                                    });
                                }
                                break;
                            }
                        }
                    });
                    if (maxOverlap !== 0) {
                        $(this).height(28 + maxOverlap * 16);
                    }
                });
            }
            $('.table-daily').show();

            assignmentStatistics(data.AssignmentStatistics);
            $('.type-row').on('click', () => {
                var assignmentStatisticsRowsState = {};
                var totalType = $(".type-row");
                totalType.each(it => {
                    if (!totalType[it].checked) {
                        assignmentStatisticsRowsState[totalType[it].name] = '';
                    } else assignmentStatisticsRowsState[totalType[it].name] = 'checked';
                });
                storeAssignmentStatisticsRowState(username, assignmentStatisticsRowsState);
                assignmentStatistics(data.AssignmentStatistics);
                $('.table-body').css({ 'margin-top': $('.sticky-filter').outerHeight() });
            });
        }
    };
})(jQuery);

jQuery(document).ready(function ($) {
    var isHostDaily = $('.body-content').attr('data-isHostDaily') === 'True';
    var today = moment().format('MM/DD/YYYY');
    var queryDate = common.urlParam('date');
    var currentDate = queryDate || today;
    var columnFilters = {};
    var today = moment(currentDate, 'MM/DD/YYYY');
    var mondayofCurrentWeek = today.startOf('isoweek');
    var fourlatestWeeks = moment(mondayofCurrentWeek.subtract(28, "days"));
    var maxQueryTime = moment(currentDate, 'MM/DD/YYYY').diff(fourlatestWeeks, 'days');

    $('.datepicker').datepicker({
        minDate: -maxQueryTime,
        firstDay: 1,
        currentText: currentDate
    }).val(currentDate);

    $('.icon-calendar').on('click', function () {
        $('.datepicker').focus();
    });

    $(window).resize(dailyWorkingShift.updateStickyWidth);

    $('#employee').on('keypress', function (event) {
        if (event.keyCode === 13) {
            $(".btn-search").trigger('click');
        }
    });

    $('body').on('focus', '.materialize-textarea', function () {
        $(this).trigger('autoresize');
    });

    // render section filter
    function initSectionFilter() {
        for (var i = 0; i < workingShiftData.sections.length; i++) {
            var section = workingShiftData.sections[i];
            section.enable = true;
            section.checked = true;
        }
        $('#section').fanexMultiSelect({
            listData: workingShiftData.sections,
            showFilter: true,
            minWidth: 120,
            customCheckAllText: 'All',
            useCustomCheckAllText: true,
            noneSelectedText: 'Select sections'
        });
    }
    initSectionFilter();

    // render location
    function initLocationFilter() {
        for (var i = 0; i < workingShiftData.userLocations.length; i++) {
            var location = workingShiftData.userLocations[i];
            location.enable = true;
            location.checked = true;
        }
        $('#userLocations').fanexMultiSelect({
            minWidth: 120,
            listData: workingShiftData.userLocations,
            showFilter: true,
            customCheckAllText: 'All',
            useCustomCheckAllText: true,
            noneSelectedText: 'Select Location'
        });
    }
    initLocationFilter();

    function initDepartmentFilter() {
        for (var i = 0; i < workingShiftData.departments.length; i++) {
            var department = workingShiftData.departments[i];
            department.enable = true;
            department.checked = true;
        }
        $('#department').fanexMultiSelect({
            minWidth: 120,
            listData: workingShiftData.departments,
            showFilter: true,
            customCheckAllText: 'All',
            useCustomCheckAllText: true,
            noneSelectedText: 'Select Location'
        });
    }
    initDepartmentFilter();

    // render shift & search
    dailyWorkingShift.init();
    dailyWorkingShift.getShiftsAndSearch();

    function hideAllPopup() {
        $(".ui-dialog:visible").find('.ui-dialog-content').dialog('close');
        $('.tipsy').remove();
    }

    $.fn.editShiftRemark = function () {
        this.click(function (event) {
            hideAllPopup();
            event.stopPropagation();
            if (!isHostDaily) {
                return;
            }

            var $this = this;
            $('.remark-popover').removeClass('is-open');

            var remarkContainer = $(this).parent();
            var remarkPopover = remarkContainer.next();
            remarkPopover.toggleClass('is-open');
            var remarkContent = remarkContainer.find('.show-remark').text();
            var textarea = remarkPopover.find('textarea');
            textarea.val(remarkContent);
            textarea.focusin();

            var closeButton = remarkPopover.find('.mdi-close');
            closeButton.off('click');
            closeButton.on('click', function () {
                remarkPopover.toggleClass('is-open');
            });
            var cancelButton = remarkPopover.find('.btn-cancel');
            cancelButton.off('click');
            cancelButton.on('click', function () {
                remarkPopover.toggleClass('is-open');
            });
            var saveButton = remarkPopover.find('.btn-save');
            saveButton.off('click');
            saveButton.on('click', function () {
                var htmlRow = $(this).closest('.staff-row');
                var userId = parseInt(htmlRow.attr('data-id'));
                var model = {
                    apiKey: 'working-shift-update-remark-daily',
                    userId: userId,
                    category: 2,
                    remark: textarea.val()
                };
                $('.loading-process').css('display', 'inline');
                dailyWorkingShiftService.updateWorkingShiftRemark(model, function () {
                    $('.loading-process').css('display', 'none');
                    remarkContainer.find('.show-remark').text(model.remark);
                    remarkContainer.find('.show-remark').attr('original-title', model.remark);
                    $($this).removeClass('has-remark');
                    if (model.remark) {
                        $($this).addClass('has-remark');
                    }
                    remarkPopover.toggleClass('is-open');
                }).error(function () {
                    $('.loading-process').css('display', 'none');
                    toastr.error('Update failure');
                });
            });
        });
    };

    $.fn.deleteUserNote = function () {
        this.click(function (event) {
            Swal.fire({
                html: 'Are you sure you want to delete this note?',
                type: 'question',
                showCancelButton: true,
                reverseButtons: true,
                animation: false,
                heightAuto: false,
                scrollbarPadding: false,
                confirmButtonText: 'OK'
            }).then((result) => {
                if (result.value) {
                    var _this = this;

                    event.preventDefault();
                    var noteItem = $(_this).closest('.note-item');
                    var noteList = $(_this).closest('.note-list');
                    var notePopover = $(_this).closest('.note-popover');
                    var noteId = parseInt(noteItem.data('id'));

                    var deleteModel = {
                        ApiKey: 'employee-working-shift-delete-note',
                        EmployeeWorkingShiftNoteId: noteId
                    };

                    dailyWorkingShiftService.deleteEmployeeWorkingShiftNote(deleteModel, function (deleteResult) {
                        noteItem.remove();
                        let noteItems = noteList.find('.note-item');
                        let noteIcon = notePopover.parent();
                        let nodataElement = notePopover.find('.no-data');
                        if (noteItems.length == 0 && nodataElement.length == 0) {
                            noteList.remove();
                            notePopover.append('<div class="no-data">There is no data</div>');
                            noteIcon.removeClass('has-note');
                        }
                    }).error(function () {
                        toastr.error('Something went wrong');
                    });
                }
            });
        });
    };

    $.fn.editUserNote = function () {
        this.click(function (event) {
            var $this = this;

            event.preventDefault();
            var noteItem = $($this).closest('.note-item');
            var noteId = parseInt(noteItem.data('id'));
            var noteItemContent = noteItem.find('.content');
            var userNoteValue = noteItemContent.find('.user-note').text();
            var noteItemContentEdit = '<div class="content-edit"><div class="text-left"><textarea class="materialize-textarea note-textinput" type="text" maxlength="4000">' + userNoteValue + '</textarea></div><div class="icon-button"><button class="icon-save mdi mdi-check"><button class="icon-cancel mdi mdi-close"></button></div>';
            noteItemContent.replaceWith(noteItemContentEdit);

            $(".working-shift-hour .note-popover .note-item .content-edit .icon-save").on("click", function (event) {
                let noteItemContentEditElement = noteItem.find('.content-edit');
                let noteEditValue = noteItemContentEditElement.find('.note-textinput').val();
                if ($.trim(noteEditValue) != '') {
                    var updateModel = {
                        ApiKey: 'employee-working-shift-update-note',
                        EmployeeWorkingShiftNoteId: noteId,
                        Note: noteEditValue
                    };
                    dailyWorkingShiftService.insertUpdateQueryEmployeeWorkingShiftNote(updateModel, function (updateResult) {
                        var queryModel = {
                            ApiKey: 'employee-working-shift-query-note-by-id',
                            EmployeeWorkingShiftNoteId: updateResult[0].NoteId
                        };
                        dailyWorkingShiftService.insertUpdateQueryEmployeeWorkingShiftNote(queryModel, function (queryResult) {
                            var displayLastUpdated = '';
                            if (queryResult[0].LastUpdated) {
                                displayLastUpdated = moment(queryResult[0].LastUpdated, 'YYYY-MM-DDTHH:mm:ss').format('MM/DD/YYYY hh:mm A');
                            }
                            let newNoteItem = '<div class="content"><div class="text-left"><strong class="user-name">@' + queryResult[0].UserName + '</strong>' + '</strong>' + '<span class="user-note">' + ' ' + (queryResult[0].Note === null ? '' : common.htmlEntities(queryResult[0].Note)) + '</div><span class="date-time">' + displayLastUpdated + '</span><div class="icon-button"><button class="icon-edit mdi mdi-pencil"></button><button class="icon-delete mdi mdi-delete"></button>';
                            noteItemContentEditElement.replaceWith(newNoteItem);
                            $('.working-shift-hour .note-popover .note-item .icon-button .icon-delete').deleteUserNote();
                            $('.working-shift-hour .note-popover .note-item .icon-button .icon-edit').editUserNote();
                        });
                    }).error(function () {
                        toastr.error('Something went wrong');
                    });
                }
            });
            $(".working-shift-hour .note-popover .note-item .content-edit .icon-cancel").on("click", function (event) {
                let noteItemContentEditElement = noteItem.find('.content-edit');
                noteItemContentEditElement.replaceWith(noteItemContent);
                $('.working-shift-hour .note-popover .note-item .icon-button .icon-delete').deleteUserNote();
                $('.working-shift-hour .note-popover .note-item .icon-button .icon-edit').editUserNote();
            });
        });
    };

    function removeColumnFilters() {
        columnFilters = {};
        $('.js-iconFilter').removeClass('is-filter');
        $('.js-inputSearchLive').val('');
    }

    function applyColumnFilters() {
        var totalRecords = 0;
        $('.staff-row').each(function (index, staffRow) {
            var filterValue = columnFilters['Staff'] ? columnFilters['Staff'].toUpperCase() : undefined;
            if (filterValue &&
                $(staffRow).find('.staff-name').text().toUpperCase().indexOf(filterValue) === -1 &&
                $(staffRow).find('.staff-team').text().toUpperCase().indexOf(filterValue) === -1 &&
                $(staffRow).find('.show-remark').text().toUpperCase().indexOf(filterValue) === -1
                ) {
                $(staffRow).hide();
                return;
            }

            filterValue = columnFilters['Group'] ? columnFilters['Group'].toUpperCase() : undefined;
            if (filterValue && $(staffRow).find('.group').text().toUpperCase().indexOf(filterValue) === -1) {
                $(staffRow).hide();
                return;
            }

            filterValue = columnFilters['Level'] ? columnFilters['Level'].toUpperCase() : undefined;
            if (filterValue && $(staffRow).find('.level').text().toUpperCase().indexOf(filterValue)) {
                $(staffRow).hide();
                return;
            }

            filterValue = columnFilters['Note'] ? columnFilters['Note'].toUpperCase() : undefined;
            if (filterValue) {
                var notes = $(staffRow).find('.user-note').map(function () {
                    return $(this).text().toUpperCase();
                }).get();
                var hasNoteMatchFilter = notes.some(function (note) {
                    return note.indexOf(filterValue) !== -1;
                });
                if (!hasNoteMatchFilter) {
                    $(staffRow).hide();
                    return;
                }
            }

            totalRecords++;
            $(staffRow).show();
        });
        $('#total-daily').text(totalRecords);
    }

    $('.js-iconFilter').click(function () {
        hideAllPopup();
        var columnName = $(this).data('column-name');
        var dialogSearchLive = $(".js-dialogSearchLive[data-column-name='" + columnName + "']");
        if (dialogSearchLive.length === 0) {
            var html = '\
                <div class="row dialog-search-live js-dialogSearchLive" data-column-name="' + columnName + '" style="display: none;"> \
                    <input class="input-search js-inputSearchLive" data-column-name="' + columnName + '" data-is-searching="0" data-old-value="" placeholder="Search..."> \
                    <a class="waves-effect waves-light btn btn-search js-btnSearchLive" data-column-name="' + columnName + '">Search</a> \
                    <a class="waves-effect waves-light clear-all js-clearAllFilter">Clear All Filters</a> \
                </div>';
            $('.body-content').append(html);
            dialogSearchLive = $(".js-dialogSearchLive[data-column-name='" + columnName + "']");
        }

        var positionDisplayDialog = $(this);
        var displayPosition = '',
            classDialog = '';
        if (positionDisplayDialog.offset().left > ($(document).scrollLeft() + $(window).width() / 2)) {
            displayPosition = 'right+3';
            classDialog = 'popup-search-left';
        } else {
            displayPosition = 'left-2';
            classDialog = 'popup-search-right';
        }

        dialogSearchLive.dialog({
            autoOpen: false,
            position: { of: positionDisplayDialog, my: displayPosition + ' top+4', at: displayPosition + ' bottom', collision: 'none' },
            resizable: false,
            draggable: false,
            closeText: 'x',
            width: '300px',
            minHeight: 0,
            dialogClass: classDialog
        });

        dialogSearchLive.dialog("open");
        dialogSearchLive.find('.js-inputSearchLive').off('keyup').on('keyup', function (e) {
            if (e.which === 13) {
                var dialog = $(this).closest('.js-dialogSearchLive');
                var searchButton = dialog.find('.js-btnSearchLive');
                searchButton.click();
            }
        });
        dialogSearchLive.find('.js-clearAllFilter').off('click').on('click', function () {
            var dialog = $(this).closest('.js-dialogSearchLive');
            removeColumnFilters();
            applyColumnFilters();
            dialog.dialog('close');
        });
        dialogSearchLive.find('.js-btnSearchLive').off('click').on('click', function () {
            var dialog = $(this).closest('.js-dialogSearchLive');
            var input = dialog.find('.js-inputSearchLive');
            var columnName = input.attr('data-column-name');
            var filterIcon = $(".js-iconFilter[data-column-name='" + columnName + "']");
            var filterValue = input.val();
            columnFilters[columnName] = filterValue;
            if (filterValue) {
                if (!filterIcon.hasClass('is-filter')) {
                    filterIcon.addClass('is-filter');
                }
            } else {
                filterIcon.removeClass('is-filter');
            }

            applyColumnFilters();
            dialog.dialog('close');
        });
    });

    $('.btn-search').click(function () {
        var sectionIds = $('#section').fanexMultiSelect('getItems', true);
        if (sectionIds.length === 0) {
            Swal.fire({
                html: 'Please select at least one section',
                type: 'error',
                animation: false,
                heightAuto: false,
                scrollbarPadding: false,
                confirmButtonText: 'OK'
            });
            return;
        }

        var userLocationCodes = $('#userLocations').fanexMultiSelect('getItems', true);
        if (userLocationCodes.length === 0) {
            Swal.fire({
                html: 'Please select at least one location',
                type: 'error',
                animation: false,
                heightAuto: false,
                scrollbarPadding: false,
                confirmButtonText: 'OK'
            });
            return;
        }

        var departmentIds = $('#department').fanexMultiSelect('getItems', true);
        if (departmentIds.length === 0) {
            Swal.fire({
                html: 'Please select at least one department',
                type: 'error',
                animation: false,
                heightAuto: false,
                scrollbarPadding: false,
                confirmButtonText: 'OK'
            });
            return;
        }

        var shifts = $('#shift').fanexMultiSelect('getItems', true);
        if (shifts.length === 0) {
            Swal.fire({
                html: 'Please select at least one shift',
                type: 'error',
                animation: false,
                heightAuto: false,
                scrollbarPadding: false,
                confirmButtonText: 'OK'
            });
            return;
        }

        removeColumnFilters();
        var shiftId = shifts.join(',');
        var uncheckedShifts = $('#shift').fanexMultiSelect('getItems', false);
        if (uncheckedShifts.length === 0) {
            shiftId = '';
        }

        var dailyModel = {
            date: $('.datepicker').val(),
            user: $('#employee').val().trim(),
            sectionId: sectionIds.join(','),
            shiftId: shiftId,
            locationCode: userLocationCodes.join(','),
            departmentId: departmentIds.join(','),
            excludeClosedAccount: $('#excludeClosedAccount').prop('checked')
        };
        $('.loading-process').css('display', 'inline');

        dailyWorkingShift.lastModel = dailyModel;
        dailyWorkingShiftService.search(dailyModel, function (result) {
            $('.loading-process').css('display', 'none');
            $('#result').html('');
            $('.txt-day').text(dailyModel.date);
            dailyWorkingShift.render(result, dailyModel.date);
            dailyWorkingShift.onClickUserNote();
            dailyWorkingShift.onClickCloseUserNote();
            dailyWorkingShift.onInsertUserNote();
            $('.working-shift-hour .note-popover .note-item .icon-button .icon-delete').deleteUserNote();
            $('.working-shift-hour .note-popover .note-item .icon-button .icon-edit').editUserNote();
            dailyWorkingShift.updateStickyWidth();
            $('.working-shift-hour .icon-remark').editShiftRemark();
            $('.working-shift-hour .tooltip-tipsy').tipsy({ gravity: 'sw' });
        }).error(function () {
            $('.loading-process').css('display', 'none');
        });
    });

    function resizeWindow() {
        $('.table-body').css({ 'margin-top': $('.sticky-filter').outerHeight() });
    }
    $(window).on('resize', function () {
        resizeWindow();
    });
    resizeWindow();
});