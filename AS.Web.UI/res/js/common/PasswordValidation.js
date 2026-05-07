var specialchars = '`~!@#$%^&*()-_+=|\\{[}]:;"\',.?/ ';
jQuery.extend({
    isFunction: function(func) {
        if (typeof func == 'function')
            return true;
        else
            return false;
    }
});
jQuery.fn.extend({
    IsHasCharacters: function(invalid_chars) {
        if (invalid_chars == null) return true;
        if (invalid_chars == '') return true;
        var invalidHexValue = '';
        for (var i = 0; i < invalid_chars.length; i++) {
            invalidHexValue += '\\x' + invalid_chars.charCodeAt(i).toString(16).toUpperCase();
        }
        return $(this).val().search(new RegExp('[' + invalidHexValue + ']', 'ig')) >= 0;
    },

    MinLength: function(len) {
        if ($(this).val().length < len)
            return false;
        else
            return true;
    },
    MaxLength: function(len) {
        if ($(this).val().length > len)
            return false;
        else
            return true;
    },
    LatestContaintNumberic: function(num) {
        if ($(this).val().replace(/[a-zA-Z]/g, '').replace(/[^\w\s]/gi, '').length < num)
            return false;
        else
            return true;
    },
    LatestContaintLowerCase: function(num) {
        if ($(this).val().replace(/[A-Z]/g, '').replace(/[0-9]/g, '').replace(/[^\w\s]/gi, '').length < num)
            return false;
        else
            return true;
    },
    LatestContaintUpperCase: function(num) {
        if ($(this).val().replace(/[a-z]/g, '').replace(/[0-9]/g, '').replace(/[^\w\s]/gi, '').length < num)
            return false;
        else
            return true;
    },
    LatestContaintSpecialChar: function(num) {
        if (num == 0) {
            if ($(this).val().replace(/[a-zA-Z]/g, '').replace(/[0-9]/g, '').length > 0)
                return false;
            else
                return true;
        }
        else if (num == -1) {
            return true;
        }
        else {
            if ($(this).val().replace(/[a-zA-Z]/g, '').replace(/[0-9]/g, '').length < num)
                return false;
            else
                return true;
        }
    },
    CheckHTMLTag: function () {
        var regex = new RegExp(/^((?![<>]).)*$/);
        return regex.test($(this).val());
    },
    CheckEncodeTag: function () {
        var regex = new RegExp(/^((?!(&#)).)*$/);
        return regex.test($(this).val());
    }
});
