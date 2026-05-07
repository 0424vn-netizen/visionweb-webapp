const pauseMerchantAlert = (function () {
    const isEditMode = FeatureMode === 'Edit' && !!$('#' + Risk_Assignment_Info_uxAssignmentName).val().trim();
    function validatePauseMerchantAlert() {
        let isValid = true;
        $('tr.Row.pause-merchant-alert-row').each(function () {
            const lbTitleDateRange = $(this).find('[data-component="lbTitleDateRange"]');
            const uxFromDate = $(this).find('[data-component="uxFromDate"]');
            const uxToDate = $(this).find('[data-component="uxToDate"]');
            const lbErrorDateRange = $(this).find('[data-component="lbErrorDateRange"]');
            const lbTitleMerchants = $(this).find('[data-component="lbTitleMerchants"]');
            const radMultiSelectEmpty = $(this).find('.k-multiselect .k-reset:empty');
            const lbErrorMerchants = $(this).find('[data-component="lbErrorMerchants"]');
        
            const fromDateInput = $find(uxFromDate[0].id);
            const toDateInput = $find(uxToDate[0].id);
            
            const fromDateValue = fromDateInput.get_selectedDate();
            const toDateValue = toDateInput.get_selectedDate();
            
            if (fromDateValue == null || toDateValue == null) {
                isValid = false;
                if ((fromDateValue == null && fromDateInput._lastSetTextBoxValue != '') || (toDateValue == null && toDateInput._lastSetTextBoxValue != '')) {
                    setError(true, lbTitleDateRange, lbErrorDateRange, 'The date format is invalid. Enter MM/DD/YYYY.');                    
                }
                else {
                    setError(true, lbTitleDateRange, lbErrorDateRange, 'This is a required field.');
                }
            }
            else {
                const current = new Date();
                const currentDate = current.setHours(0, 0, 0, 0);
                const fromDate = new Date(fromDateValue).setHours(0, 0, 0, 0);
                const toDate = new Date(toDateValue).setHours(0, 0, 0, 0);

                if (!isEditMode && (fromDate < currentDate || toDate < currentDate)) {
                    isValid = false;
                    setError(true, lbTitleDateRange, lbErrorDateRange, 'The date range has expired. Adjust date range or remove this condition.');
                }
                else {
                    if (toDate < fromDate) {
                        isValid = false;
                        setError(true, lbTitleDateRange, lbErrorDateRange, 'The end date must be greater than or equal to the begin date.');
                    }
                    else {
                        setError(false, lbTitleDateRange, lbErrorDateRange, '');
                    }
                }
            }

            if ($(radMultiSelectEmpty).length > 0) {
                isValid = false;
                setError(true, lbTitleMerchants, lbErrorMerchants, 'This is a required field.');
            }
            else {
                setError(false, lbTitleMerchants, lbErrorMerchants, '');
            }
        });

        if (!isValid) {
            $('#lbTitleSessionPauseMerchantAlert').css('color', 'red');
        }
        else {
            $('#lbTitleSessionPauseMerchantAlert').css('color', '#3e3e3e');
        }

        return isValid;
    }

    function setError(isError, titleElement, messageElement, errorMessage) {
        if (isError) {
            $(titleElement).css('color', 'red');
            $(messageElement).html(errorMessage);
        } else {
            $(titleElement).css('color', '#3e3e3e');
            $(messageElement).html('');
        }
    }
  
    function disableAllMerchantsViewMode() {
        const result = $('#' + Risk_Assignment_Filters_hddIsViewMode_ClientID).val();
        const isViewMode = (result == 'true');
        if (isViewMode) {
            disableAllMerchantSelects();
        }
    }

    // Fix multiple clicks remove causing errors
    $(document).on('click', '.remove-condition', function () {
        $(this).css('pointer-events', 'none');
    });

    return {
        validatePauseMerchantAlert: validatePauseMerchantAlert,
        disableAllMerchantsViewMode: disableAllMerchantsViewMode
    };
})();