
function ValidateSourceHierarchy() {
    var result = 'none';
    var leadSource = $get(rm_MCF_Assignment_Filter_Lead_Source);
    var referralSource = $get(rm_MCF_Assigment_Filter_Referal_Source);
    if ((leadSource != undefined && (leadSource.textContent.trim() != 'N/A' && leadSource.textContent.trim() != ""))||
       (referralSource != undefined && (referralSource.textContent.trim() != 'N/A' && referralSource.textContent.trim() != ""))) {
        result = 'valid';
    }
    return result;
}