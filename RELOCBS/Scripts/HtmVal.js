function CheckHtml(val) {

    var reg = /<(.|\n)*?>/g;

    return reg.test(val);
}
$(document).on("keydown", ":input:not(button):not(submit)", function (e) {
    if (e.which == 13) {
        e.preventDefault();
    }
});
$(document).ready(function () {

    $("input[type='text'], textarea, .summernote").focusout(function (e) {
        var reg = /<(.|\n)*?>/g;
        var message = $(this).val();
        //if (reg.test($('input[type=text],select').val()) == true)
        if (/<(br|basefont|hr|input|source|frame|param|area|meta|!--|col|link|option|base|img|wbr|!DOCTYPE|a|abbr|acronym|address|applet|article|aside|audio|b|bdi|bdo|big|blockquote|body|button|canvas|caption|center|cite|code|colgroup|command|datalist|dd|del|details|dfn|dialog|dir|div|dl|dt|em|embed|fieldset|figcaption|figure|font|footer|form|frameset|head|header|hgroup|h1|h2|h3|h4|h5|h6|html|i|iframe|ins|kbd|keygen|label|legend|li|map|mark|menu|meter|nav|noframes|noscript|object|ol|optgroup|output|p|pre|progress|q|rp|rt|ruby|s|samp|script|section|select|small|span|strike|strong|style|sub|summary|sup|table|tbody|td|textarea|tfoot|th|thead|time|title|tr|track|tt|u|ul|var|video).*?>|<(video).*?<\/\2>/i.test(message) == true) {
            alert('HTML Tag are not allowed');
        }
        e.preventDefault();
    });

    ////Select 2 dropdonw with enable and disable class
    $('.enableselect').select2();
    $('.disableselect').prop('disabled', true);
    //$('.disableselect').select2().enable(false);
    //$('input[type="submit"]').removeAttr('disabled', 'disabled');
    $('.Showspinner').click(
		function () {
		    $('#overlay').fadeIn();
		    setTimeout(function () {
		        $('.field-validation-error').each(function () {
		            if ($(this).length > 0) {
		                $('#overlay').fadeOut();
		            }
		        });
		    }, 2000);
		}
	);
    $('#overlay').delay(2000).fadeOut();
});

function AjaxFillDropDown(control, url, strsearch) {


    //$(control).empty().trigger('change');
    if (strsearch) {
        var DrpSelect = $(control);
        $.ajax({
            type: 'GET',
            url: url,
            data: { Value: strsearch }
        }).then(function (data) {

            if (data.CountryList.length > 0) {
                // create the option and append to Select2
                var option = new Option(data.CountryList[0].Text, data.CountryList[0].Value, true, true);
                DrpSelect.append(option).trigger('change');

                // manually trigger the `select2:select` event
                DrpSelect.trigger({
                    type: 'select2:select',
                    params: {
                        data: data.CountryList
                    }
                });
            }

        });
    }
    else {
        $(control).select2({
            minimumInputLength: 3,
            //width: 'resolve',
            placeholder: "Select One",
            ajax: {
                url: url, // Controller - Select2Demo and Action -AccessRemoteData
                type: "POST",
                dataType: 'json',
                data: function (term) {
                    return {
                        term: term.term,
                    };
                },
                processResults: function (data) {
                    //$(control).find('option').remove();
                    return {
                        results: $.map(data.CountryList, function (item) {

                            return {
                                id: item.Value,
                                text: item.Text
                            }
                        })
                    }; // data.CountryList returning json data from Controlle
                }
            }
        });
        //$(control).parent().find('span')
        //	.removeClass('select2-container')
        //	.css("width", "200px")
        //	//.css("flex-grow", "1")
        //	//.css("box-sizing", "border-box")
        //	.css("display", "inline-block")
        //	.css("margin", "0")
        //	//.css("position", "relative")
        //	.css("vertical-align", "middle")
    }

}

$('select').on('classChange', function () {
    if ($(this).hasClass('disableselect')) {
        $(this).select2();
    }
    else if ($(this).hasClass('enableselect')) {
        $(this).prop('disabled', true);
    }
});

function AjaxFillConvRate(url, ConvRateCtrl) {
    $.get(url, function (data) {
        if (data.ConvRate > 0) {
            $(ConvRateCtrl).val(data.ConvRate);
        }
    });
}

var select2_open;
// open select2 dropdown on focus
$(document).on('focus', '.select2-selection--single', function (e) {
    select2_open = $(this).parent().parent().siblings('select');
    select2_open.select2('open');
});



// fix for ie11
if (/rv:11.0/i.test(navigator.userAgent)) {
    $(document).on('blur', '.select2-search__field', function (e) {
        select2_open.select2('close');
    });
}




//////html table to json

function formToJSON(table) {//begin function


    //array to hold the key name
    var keyName;

    //array to store the keyNames for the objects
    var keyNames = [];

    //array to store the objects
    var objectArray = [];


    //get the number of cols
    var numOfCols = table.rows[0].cells.length;

    //get the number of rows
    var numOfRows = table.rows.length;

    //add the opening [ array bracket
    objectArray.push("[");



    //loop through and get the propertyNames or keyNames
    for (var i = 0; i < numOfCols; i++) {//begin for loop  

        //store the html of the table heading in the keyName variable
        keyName = table.rows[0].cells[i].innerHTML;

        //add the keyName to the keyNames array
        keyNames.push(keyName);

    }//end for loop



    //loop through rows
    for (var i = 1; i < numOfRows; i++) {//begin outer for loop    

        //add the opening { object bracket
        objectArray.push("{\n");

        for (var j = 0; j < numOfCols; j++) {//begin inner for loop   

            //extract the text from the input value in the table cell
            var inputValue = "";

            if (table.rows[i].cells[j].getElementsByTagName("input").length > 0) {

                inputValue = table.rows[i].cells[j].children[0].value;
            }
            else if (table.rows[i].cells[j].innerHTML != null && table.rows[i].cells[j].innerHTML != "") {

                inputValue = table.rows[i].cells[j].innerHTML.trim();
            }

            if (inputValue != "") {

                //store the object keyNames and its values
                objectArray.push("\"" + keyNames[j] + "\":" + "\"" + inputValue + "\"");

                //if j less than the number of columns - 1(<-- accounting for 0 based arrays)
                if (j < (numOfCols - 1)) {//begin if then

                    //add the , seperator
                    objectArray.push(",\n");

                }//end if then    

            }



        }//end inner for loop

        //if i less than the number of rows - 1(<-- accounting for 0 based arrays)
        if (i < (numOfRows - 1)) {//begin if then

            //add the closing } object bracket followed by a , separator
            objectArray.push("\n},\n");

        }
        else {

            //add the closing } object bracket
            objectArray.push("\n}");

        }//end if then else

    }//end outer for loop

    //add the closing ] array bracket
    objectArray.push("]");

    return objectArray.join("");


}//end function





// Plain textbox keypress events start //
// onkeypress = "return OnlyInteger(this,event,'U' or 'L' or '')"

function OnlyFreeText(s, e, charcase) {
    var keynum;
    var keychar;
    var numcheck;
    if (window.event) // IE
    {
        keynum = e.keyCode;
    }
    else if (e.which) // Netscape/Firefox/Opera
    {
        keynum = e.which;
    }
    if (!keynum) return true;
    if (keynum == 13 || keynum == 10 || keynum == 8) return true;
    keychar = String.fromCharCode(keynum);
    var ValidText;
    ValidText = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890123456789{}[]-=()!@$%^*?~`,.+_/ ";
    if (ValidText.indexOf(keychar) >= 0) {
        if (charcase == 'U' && (keynum >= 97 && keynum <= 122)) {
            s.value = s.value + keychar.toUpperCase();
            return false;
        }
        else if (charcase == 'L' && (keynum >= 65 && keynum <= 90)) {
            s.value = s.value + keychar.toUpperCase();
            return false;
        }
        return true;
    }
    else
        return false;
}

function OnlyCode(s, e) {
    var keynum;
    var keychar;
    var numcheck;
    if (window.event) // IE
    {
        keynum = e.keyCode;
    }
    else if (e.which) // Netscape/Firefox/Opera
    {
        keynum = e.which;
    }
    if (!keynum) return true;
    if (keynum == 13 || keynum == 10 || keynum == 8) return true;
    keychar = String.fromCharCode(keynum);
    var ValidText = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    if (ValidText.indexOf(keychar) >= 0) {
        if (keynum >= 97 && keynum <= 122) {
            s.value = s.value + keychar.toUpperCase();
            return false;
        }
        return true;
    }
    else
        return false;
}

function OnlyAlpha(s, e, charcase) {
    var keynum;
    var keychar;
    var numcheck;
    if (window.event) // IE
    {
        keynum = e.keyCode;
    }
    else if (e.which) // Netscape/Firefox/Opera
    {
        keynum = e.which;
    }
    if (!keynum) return true;
    if (keynum == 13 || keynum == 10 || keynum == 8) return true;
    keychar = String.fromCharCode(keynum);
    var ValidText;
    ValidText = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-_() ";

    if (ValidText.indexOf(keychar) >= 0) {
        if (charcase == 'U' && (keynum >= 97 && keynum <= 122)) {
            s.value = s.value + keychar.toUpperCase();
            return false;
        }
        else if (charcase == 'L' && (keynum >= 65 && keynum <= 90)) {
            s.value = s.value + keychar.toUpperCase();
            return false;
        }
        return true;
    } else
        return false;
}

function OnlyAlphaNumeric(s, e, charcase) {
    var keynum;
    var keychar;
    var numcheck;
    if (window.event) // IE
    {
        keynum = e.keyCode;
    }
    else if (e.which) // Netscape/Firefox/Opera
    {
        keynum = e.which;
    }
    if (!keynum) return true;
    if (keynum == 13 || keynum == 10 || keynum == 8) return true;
    keychar = String.fromCharCode(keynum);
    var ValidText;
    ValidText = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_() ";

    if (ValidText.indexOf(keychar) >= 0) {
        if (charcase == 'U' && (keynum >= 97 && keynum <= 122)) {
            s.value = s.value + keychar.toUpperCase();
            return false;
        }
        else if (charcase == 'L' && (keynum >= 65 && keynum <= 90)) {
            s.value = s.value + keychar.toUpperCase();
            return false;
        }
        return true;
    } else
        return false;
}


function OnlyInteger(s, e) {
    var keynum;
    var keychar;
    var numcheck;
    if (window.event) // IE
    {
        keynum = e.keyCode;
    }
    else if (e.which) // Netscape/Firefox/Opera
    {
        keynum = e.which;
    }
    if (!keynum) return true;
    if (keynum == 13 || keynum == 10 || keynum == 8) return true;
    keychar = String.fromCharCode(keynum);
    var ValidText = "0123456789";
    if (ValidText.indexOf(keychar) >= 0)
        return true;
    else
        return false;
}


function OnlyDouble(s, e, decimalplaces) {

    if (!decimalplaces) decimalplaces = 2;
    var keynum;
    var keychar;
    var numcheck;
    if (window.event) // IE
    {
        keynum = e.keyCode;
    }
    else if (e.which) // Netscape/Firefox/Opera
    {
        keynum = e.which;
    }
    if (!keynum) return true;
    if (keynum == 13 || keynum == 10 || keynum == 8) return true;
    keychar = String.fromCharCode(keynum);
    var ValidText = "0123456789.-";
    if (keynum == 45) {

        try {

            if (keychar == '-' || s.value.indexOf("-") > 0) {

                var patt = s.value.split("-");
                if (patt.length <= 1)
                    return true;
                else
                    return false;
            }

        } catch (e) {

            return true
        }


    }


    if (ValidText.indexOf(keychar) >= 0) {
        try {
            if (keychar != '.' || s.value.indexOf(".") < 0) {
                var patt = s.value.split(".");
                if (patt[1].length < decimalplaces)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        catch (err) {
            return true;
        }
    }
    else
        return false;
}


function OnlyPhoneNumber(s, e) {
    var keynum;
    var keychar;
    var numcheck;
    if (window.event) // IE
    {
        keynum = e.keyCode;
    }
    else if (e.which) // Netscape/Firefox/Opera
    {
        keynum = e.which;
    }
    if (!keynum) return true;
    if (keynum == 13 || keynum == 10 || keynum == 8) return true;
    keychar = String.fromCharCode(keynum);
    var ValidText = "0123456789-";
    if (ValidText.indexOf(keychar) >= 0)
        return true;
    else
        return false;
}

function GetMonthNameShort(Month) {
    if (Month == 1) return 'Jan';
    else if (Month == 2) return 'Feb';
    else if (Month == 3) return 'Mar';
    else if (Month == 4) return 'Apr';
    else if (Month == 5) return 'May';
    else if (Month == 6) return 'Jun';
    else if (Month == 7) return 'Jul';
    else if (Month == 8) return 'Aug';
    else if (Month == 9) return 'Sep';
    else if (Month == 10) return 'Oct';
    else if (Month == 11) return 'Nov';
    else if (Month == 12) return 'Dec';
    else return '';
}


$('.customTxt').keypress(function (e) {
    if (e.which == 13) {
        var control = e.target;
        var controlHeight = $(control).height();
        $(control).height(controlHeight + 17);
    }
});

$('.customTxt').blur(function (e) {
    var textLines = $(this).val().trim().split(/\r*\n/).length;
    $(this).val($(this).val().trim()).height(textLines * 17);
});


