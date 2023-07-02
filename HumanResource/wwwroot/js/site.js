// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


$(function () {
    $("#loaderbody").addClass('hide');

    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass('hide');
    }).bind('ajaxStop', function () {
        $("#loaderbody").addClass('hide');
    });
});

showInPopup = (url, title) => {
    try {
        $.ajax({
            type: 'GET',
            url: url,
            cache: false,
            contentType: false,
            processData: false,
            dataType: "HTML",
            success: function (res) {
                $('#form-modal .modal-body').html(res);
                $('#form-modal .modal-title').html(title);
                $('#form-modal').modal('show');
                // to make popup draggable
                $('.modal-dialog').draggable({
                    handle: ".modal-header"
                });
            }
        })
        //to prevent default form submit event
        return false;
    }
    catch (ex) {
        console.log(ex)
    }
}


jQueryAjaxPost = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#view-all').html(res.html)
                    $('#form-modal .modal-body').html('');
                    $('#form-modal .modal-title').html('');
                    $('#form-modal').modal('hide');
                    //$.notify('Submit Successfully ', globalPostion: 'top center' , className= 'success');
                }
                else
                    $('#form-modal .modal-body').html(res.html);
            },
            error: function (err) {
                console.log(err.responseText);
                console.log(err);
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

jQueryAjaxDelete = form => {
    if (confirm('Are you sure to delete this record ?')) {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    $('#view-all').html(res.html);
                },
                error: function (xhr, status, error) {
                    alert(xhr.responseText);
                    console.log(xhr)
                }
                //error: function (err) {
                //    console.log(err)
                //}
            })
        } catch (ex) {
            console.log(ex)
        }
    }

    //prevent default form submit event
    return false;

   
}

function Change(element) {
    var Categoryid = $(element).find('option:selected').val();
    var Item = $(element).closest('tr').find('[id*=ItemCodes]').empty();
    $(Item).html('<option value="">--Select Item--</option>');
    $.ajax({
        url: 'GetItems',
        data: { CategoryID: Categoryid },
        type: 'GET',
        success: function (data) {
            var items = '<option value="">--Select Item--</option>';
            $.each(data, function (i, Item) {
                items += "<option value='" + Item.value + "'>" + Item.text + "</option>";
            });
            $(Item).html(items);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}

function DeleteItem(btn) {

    var table = document.getElementById('RequestDetailsTable');
    var rows = table.getElementsByTagName('tr');

    if (rows.length == 2) {
        alert("This row Cannot deleted");
        return;
    }

    var btnIndx = btn.id.replaceAll('btnremove-', '');
    var idofIsDeleted = "IsDeleted-" + btnIndx;
    var hidIsDeleted = document.querySelector("[id$='" + idofIsDeleted + "']").id;
    hidIsDeleted.value = "true";

    //var idOfQuantity = btnIndx + "__ItemQuantity";
    //var txtQuantity = document.querySelector("[id$='" + idOfQuantity + "']");
    //txtQuantity.value = 1;

    //var lstItemId = "lstItemId-" + btnIndx;
    //var txtlstItemId = document.querySelector("[id$='" + lstItemId + "']");
    //txtlstItemId.value = 1;

    

    //var idOfCategory = "lstCategoryCtrl-" + btnIndx;
    //var txtCategory = document.querySelector("[id$='" + idOfCategory + "']");
    //txtCategory.value =1;

 

    $(btn).closest('tr').hide();

    //$(btn).closest('tr').remove();

}

function AddItem(btn) {

    var table = document.getElementById("RequestDetailsTable");
    var rows = table.getElementsByTagName("tr");
    var rowOuterHtml = rows[rows.length - 1].outerHTML;

    var lastrowIdx = rows.length - 2;
    var nextrowIdx = eval(lastrowIdx) + 1;

    rowOuterHtml = rowOuterHtml.replaceAll('_' + lastrowIdx + '_', '_' + nextrowIdx + '_');
    rowOuterHtml = rowOuterHtml.replaceAll('[' + lastrowIdx + ']', '[' + nextrowIdx + ']');
    rowOuterHtml = rowOuterHtml.replaceAll('-' + lastrowIdx, '-' + nextrowIdx);

    var newRow = table.insertRow();
    newRow.innerHTML = rowOuterHtml;

    var x = document.getElementsByTagName("input");

    for (var cnt = 0; cnt < x.length; cnt++) {

        if (x[cnt].type == "text" && x[cnt].id.indexOf('_' + nextrowIdx + '_') > 0)
            x[cnt].value = '';

        else if (x[cnt].type == "number" && x[cnt].id.indexOf('_' + nextrowIdx + '_') > 0)
            x[cnt].value = 0;


    }



    rebindvalidator();

}

function rebindvalidator() {
    var $form = $("#ApplicationForm");
    $form.unbind();
    $form.data("validator", null);
    $.validator.unobtrusive.parse($form);
    $form.validate($form.data("unobtrusiveValidation").options);
}

function FillItems(lstCategoryCtrl, lstItemId) {

        var lstItems = $("#" + lstItemId);// $(lstItemId);
        lstItems.empty();

        var selectedCountry = lstCategoryCtrl.options[lstCategoryCtrl.selectedIndex].value;

        if (selectedCountry != null && selectedCountry != '') {
            $.getJSON("/Store/GetItemsByCategory", { CategoryCode: selectedCountry }, function (cities) {
                if (cities != null && !jQuery.isEmptyObject(cities)) {
                    $.each(cities, function (index, item) {
                        lstItems.append($('<option/>',
                            {
                                value: item.value,
                                text: item.text
                            }));
                    });
                };
            });
        }

        return;
}

function VerifyQuantity(Quantity, lstItemId) {
    var lstItems = $("#" + lstItemId);
    var message = $(Quantity).closest('tr').find('[id*=validationMessageId]');
    var itemId = $(lstItems).val();
    var ItemQuantity = Quantity.value;

    if (ItemQuantity != null && ItemQuantity != '') {
        $.getJSON("/Store/VerifyQuantity", { ItemQuantity: ItemQuantity, ItemCode: itemId }, function (data) {
            if (data == false) {
                //message.text("The Item Quantity must be less than store available");
                $(Quantity).focus();
                $(Quantity).addClass("error"); // Add a CSS class to indicate the error
                $('input[type="submit"]').attr('disabled', 'disabled');
                $('#AddCategory').attr('disabled', 'disabled');
                alert("The Item Quantity must be less than store available");
                $('#Quantity').prop('disabled', true);

            } else {
                message.text(""); // Clear the error message
                $(Quantity).removeClass("error"); // Remove the CSS class if previously added
                $('input[type="submit"]').removeAttr('disabled');
                $('#AddCategory').removeAttr('disabled');

            }
        });
    } else {
        message.text(""); // Clear the error message if the quantity is empty
        $(Quantity).removeClass("error"); // Remove the CSS class if previously added
    }
}

function FillRetrivalItems(lstReqCtrl, lstItemId) {

    var lstItems = $("#" + lstItemId);
    lstItems.empty();



    var selectedItems = lstReqCtrl.options[lstReqCtrl.selectedIndex].value;

    if (selectedItems != null && selectedItems != '') {
        $.getJSON("/Store/GetItemsByRequest", {reqNo: selectedItems  }, function (cities) {
            if (cities != null && !jQuery.isEmptyObject(cities)) {
                $.each(cities, function (index, item) {
                    lstItems.append($('<option/>',
                        {
                            value: item.value,
                            text: item.text
                        }));
                });
            };
        });
    }

    return;
}

$('input[type="radio"]').change(function () {

    var item = $(this).closest('tr').find('[id *= ItemQutReceived]');
    if ($(this).val() == 2) {
        item.val(0);
        item.prop('disabled', true);
        //.value = 0;
    } else {
        item.prop('disabled', false);
    }
});

