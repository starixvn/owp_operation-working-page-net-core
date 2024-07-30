jQuery(document).ready(function ($) {
    const validFileExtensions = new Array(".xlsx");
    $('#ExcelFile').change(function () {
        $(".upload-successfully").hide();
        $("#modalError").hide();
        $(".excel-validate-error").hide();
        $(".excel-validate-error tbody").empty();
        $("#btn-confirm-upload").hide();

        var fileExt = $(this).val();
        if (fileExt) {
            fileExt = fileExt.substring(fileExt.lastIndexOf('.'));
            if (validFileExtensions.indexOf(fileExt) < 0) {
                toastr.error('Invalid file selected, valid files are of ' + validFileExtensions.toString() + ' types.');
                return false;
            }
            else {
                return true;
            }
        }

        return false;
    });

    $('#btn-open-upload').on('click', function () {
        $.blockUI({
            message: $('.upload-working-shift'),
            css: { position: 'absolute', top: '125px', left: '330px', width: '55rem', border: '0', 'text-align': 'left', cursor: 'default' },
            overlayCSS: { cursor: 'default' }
        });
        //$('.upload-working-shift').addClass('is-open');
        resetUploadForm();
    });

    $("#btn-validate-upload").on('click', function () {
        $(".upload-successfully").hide();

        $("#modalError").hide();
        $(".excel-validate-error").hide();
        $(".excel-validate-error tbody").empty();

        var input = document.getElementById("ExcelFile");
        if (input.files.length === 0) {
            showError(workingShiftData.messages.noFile);
            return false;
        }

        if (!checkFile(input)) {
            showError(workingShiftData.messages.notAnExcelFile);
            return false;
        }

        if (input.files[0].size / (1024 * 1024) > 5) {
            showError(workingShiftData.messages.exceedMaxFileSize);
            return false;
        }

        $('.loading-process').css("display", "inline");
        $(".upload-successfully").hide();
        $("#modalError").hide();
        $("#btn-confirm-upload").hide();
        var data = new FormData();
        data.append("file", input.files[0]);

        workingShiftService.ValidateWorkingShift(data, function (obj) {
            $('.loading-process').css("display", "none");

            if (obj.InvalidCells !== null && obj.InvalidCells.length > 0) {
                $(".excel-validate-error").show();

                for (var i = 0; i < obj.InvalidCells.length; i++) {
                    var cell = obj.InvalidCells[i];
                    $(".excel-validate-error tbody").append("<tr><td>" + cell.Address + "</td><td>" + cell.Message + "</td></tr>");
                }
            }
            else {
                $(".upload-successfully").show();
                $('#btn-confirm-upload').show();
                $('#btn-confirm-upload').prop('disabled', false);
                $("#modalSuccessContent").text("All content are valid");
            }

            $("#WorkingShift").val(JSON.stringify(obj.Shifts));
        }).error(function (a, b, c) {
            $('.loading-process').css('display', 'none');
            $("#modalError").show();
            $("#modalErrorContent").text("Validate failure");
        });
    });

    $("#btn-cancel-upload").on('click', function () {
        $.unblockUI();
        resetUploadForm();
    });

    function resetUploadForm() {
        $(".upload-successfully").hide();
        $("#modalError").hide();
        $(".excel-validate-error").hide();
        $(".excel-validate-error tbody").empty();
        $("#btn-confirm-upload").hide();
        $("#ExcelFile").val("");
        $("#fileWrapper").val("");
    }

    function showError(msg) {
        $("#modalErrorContent").text(msg);
        $("#modalError").show();
    }

    function checkFile(sender) {
        var fileExt = sender.value;
        fileExt = fileExt.substring(fileExt.lastIndexOf('.'));
        if (validFileExtensions.indexOf(fileExt) < 0) {
            return false;
        }

        return true;
    }

    $("#btn-confirm-upload").on('click', function () {
        $('.loading-process').css('display', 'inline');
        $('#btn-confirm-upload').prop('disabled', true);
        $(".excel-validate-error").hide();
        $(".upload-successfully").hide();
        $("#modalError").hide();
        var shifts = $("#WorkingShift").val();
        workingShiftService.ImportWorkingShift(shifts, function (obj) {
            $('.loading-process').css('display', 'none');
            $(".upload-successfully").show();
            $("#modalSuccessContent").text("Uploaded succesfully");
            $.unblockUI();
            Swal.fire({
                text: 'Uploaded succesfully',
                type: 'success',
                animation: false,
                backdrop: false,
                onClose: () => {
                    window.location.reload();
                }
            })
        }).error(function () {
            $('.loading-process').css('display', 'none');
            $("#modalError").show();
            $("#modalErrorContent").text("There is an error when uploading file");
        });
    });
});