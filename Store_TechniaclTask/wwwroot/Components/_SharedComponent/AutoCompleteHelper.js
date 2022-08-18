function CreateCommonAutoComplete(_Configration) {
    if (_Configration.DataToSent == undefined) _Configration = JsonConcat(_Configration, { DataToSent: () => { return ($(_Configration.EffectedElmentSelector).parents("form")?.serialize() ?? ""); } })

     let DefaultAutoCompleteOptions = {
        source: function (request, response) {
            $.ajax({
                url: _Configration.Url,
                dataType: "json",
                //data: {
                //    Search: request.term
                //},
                data: _Configration.DataToSent() + "&" + decodeURIComponent($.param({ Search: request.term })),
                success: function (res) {
                    response(res.data);
                }
            });
        },
        minLength: 2,
        change: (event, ui) => {
            //if ($(event.currentTarget).val().trim() != $(_Configration.EffectedElmentSelector).data("text")?.trim() && $(_Configration.EffectedElmentSelector).val() > 0) {
            //    $(_Configration.EffectedElmentSelector).val("");
            //    $(_Configration.EffectedElmentSelector).removeAttr("data-text");
            //    $(event.currentTarget).val("")
            //    //FireDropDownChangeEvent(event.currentTarget)
            //    $(_Configration.EffectedElmentSelector).change();
            //}
        },
        select: (event, ui) => {
            $(_Configration.EffectedElmentSelector).val(ui.item.id);
            $(_Configration.EffectedElmentSelector).attr("data-text", ui.item.value);
            $(_Configration.EffectedElmentSelector).change();
            //FireDropDownChangeEvent(_Configration.EffectedElmentSelector)

        }
    }
    $(_Configration.AutoCompleteSelector).on("keydown", () => {
        $(_Configration.EffectedElmentSelector).val("");
        $(_Configration.EffectedElmentSelector).attr("");
        //$(_Configration.EffectedElmentSelector).change();
        FireDropDownChangeEvent(_Configration.EffectedElmentSelector)
    });
    return $(_Configration.AutoCompleteSelector).autocomplete(JsonConcat(DefaultAutoCompleteOptions, _Configration));
}


function CreateCustomersAutoCompleteInput(_Configration = {}) {
    debugger
    if (_Configration.AutoCompleteSelector == undefined) _Configration = JsonConcat(_Configration, { AutoCompleteSelector: "#SearchCustomerInput" })
    if (_Configration.CustomerElementSelector == undefined) _Configration = JsonConcat(_Configration, { CustomerElementSelector: "[name='CustomerID']" })
    if (_Configration.Url == undefined) _Configration = JsonConcat(_Configration, { Url: "/Customers/LoadCustomers_Search" })
    if (_Configration.DataToSent == undefined) _Configration = JsonConcat(_Configration, { DataToSent: () => { return ($(_Configration.CustomerElementSelector).parents("form")?.serialize() ?? ""); } })
    let DefaultAutoCompleteOptions = {
        source: function (request, response) {
            $.ajax({
                url: _Configration.Url,
                dataType: "json",
                //data: {
                //    Search: request.term
                //},
                data: _Configration.DataToSent() + "&" + decodeURIComponent($.param({ Search: request.term })),
                success: function (res) {
                    response(res.data);
                }
            });
        },
        minLength: 2,
        change: (event, ui) => {
            if ($(event.currentTarget).val().trim() != $(_Configration.CustomerElementSelector).data("text")?.trim() && $(_Configration.CustomerElementSelector).val() > 0) {
                $(_Configration.CustomerElementSelector).val("");
                $(_Configration.CustomerElementSelector).removeAttr("data-text");
                $(event.currentTarget).val("")
                //FireDropDownChangeEvent(event.currentTarget)
                $(_Configration.CustomerElementSelector).change();
            }
        },
        select: (event, ui) => {
            $(_Configration.CustomerElementSelector).val(ui.item.id);
            $(_Configration.CustomerElementSelector).attr("data-text", ui.item.value);
            $(_Configration.CustomerElementSelector).change();
            //FireDropDownChangeEvent(_Configration.CustomerElementSelector)

        }
    }
    $(_Configration.AutoCompleteSelector).on("keydown", () => {
        $(_Configration.CustomerElementSelector).val("");
        $(_Configration.CustomerElementSelector).attr("");
        //$(_Configration.CustomerElementSelector).change();
        FireDropDownChangeEvent(_Configration.CustomerElementSelector)
    });
    return $(_Configration.AutoCompleteSelector).autocomplete(JsonConcat(DefaultAutoCompleteOptions, _Configration));
}
function ResetCardSerialInput() {
    $("#SearchCardSerialInput").val("")
    $("[name='CardSerialID']").val("")
}
function ResetCustomerInput() {

    $("#SearchCustomerInput").val("")
    $("[name='CustomerID']").val("")
}
function ResetCardNumberInput() {
    $("#SearchCardNumberInput").val("")
    $("[name='CardNumberID']").val("")
}
function CreateCardSerialAutoCompleteInput(_Configration = {}) {
    if (_Configration.AutoCompleteSelector == undefined) _Configration = JsonConcat(_Configration, { AutoCompleteSelector: "#SearchCardSerialInput" });
    if (_Configration.CardSerialElementSelector == undefined) _Configration = JsonConcat(_Configration, { CardSerialElementSelector: "[name='CardSerialID']" });
    if (_Configration.Url == undefined) _Configration = JsonConcat(_Configration, { Url: "/CardSerials/LoadCardSerialsNotConnectedToNumber_Search" });
    if (_Configration.DataToSent == undefined) _Configration = JsonConcat(_Configration, { DataToSent: () => { return $(_Configration.CardSerialElementSelector).parents("form")?.serialize() ?? ""; } })
    let DefaultAutoCompleteOptions = {
        source: function (request, response) {
            $.ajax({
                url: _Configration.Url,
                dataType: "json",
                //data: {
                //    Search: request.term
                //},
                data: _Configration.DataToSent() + "&" + decodeURIComponent($.param({ Search: request.term })),
                success: function (res) {
                    response(res.data);
                }
            });
        },
        minLength: 2,
        change: (event, ui) => {
        },
        select: (event, ui) => {
            $(_Configration.CardSerialElementSelector).val(ui.item.id);
            $(_Configration.CardSerialElementSelector).attr("data-text", ui.item.value);
            $(_Configration.CardSerialElementSelector).change();
        }
    }
    $(_Configration.AutoCompleteSelector).on("keydown", () => {
        $(_Configration.CardSerialElementSelector).val("");
        $(_Configration.CardSerialElementSelector).attr("");
        //$(_Configration.CardSerialElementSelector).change();
        FireDropDownChangeEvent(_Configration.CardSerialElementSelector)
    });
    return $(_Configration.AutoCompleteSelector).autocomplete(JsonConcat(DefaultAutoCompleteOptions, _Configration));
}
function CreateCardNumberAutoCompleteInput(_Configration = {}) {
    if (_Configration.AutoCompleteSelector == undefined) _Configration = JsonConcat(_Configration, { AutoCompleteSelector: "#SearchCardNumberInput" })
    if (_Configration.CardNumberElementSelector == undefined) _Configration = JsonConcat(_Configration, { CardNumberElementSelector: "[name='CardNumberID']" })
    if (_Configration.Url == undefined) _Configration = JsonConcat(_Configration, { Url: "/CardsNumbers/LoadNumbersNotConnectedToSerial_Search" })
    if (_Configration.DataToSent == undefined) _Configration = JsonConcat(_Configration, { DataToSent: () => { return $(_Configration.CardNumberElementSelector).parents("form")?.serialize() ?? ""; } })
    let DefaultAutoCompleteOptions = {
        source: function (request, response) {
            $.ajax({
                url: _Configration.Url,
                dataType: "json",
                //data: {
                //    Search: request.term
                //},
                data: _Configration.DataToSent() + "&" + decodeURIComponent($.param({ Search: request.term })),
                success: function (res) {
                    response(res.data);
                }
            });
        },
        minLength: 2,
        change: (event, ui) => {
        },
        select: (event, ui) => {
            $(_Configration.CardNumberElementSelector).val(ui.item.id);
            $(_Configration.CardNumberElementSelector).attr("data-text", ui.item.value);
            $(_Configration.CardNumberElementSelector).change();
        }
    }
    $(_Configration.AutoCompleteSelector).on("keydown", () => {
        $(_Configration.CardNumberElementSelector).val("");
        $(_Configration.CardNumberElementSelector).attr("");
        //$(_Configration.CardNumberElementSelector).change();
        FireDropDownChangeEvent(_Configration.CardNumberElementSelector)
    });
    return $(_Configration.AutoCompleteSelector).autocomplete(JsonConcat(DefaultAutoCompleteOptions, _Configration));
}