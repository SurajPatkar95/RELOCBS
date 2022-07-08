function GetSurveyCostHeadXML(btnRevenueValue, btnCostValue, button, data, CompanyID) {
    $(this).closest("#MoveSOtable > TBODY > tr").each(function () {
        var CostHeadID = $(this).find('.tdNone .TblCostHead').val();
        var RateCompID = $(this).find('.tdNone .TblRateComp').val();
        var Detail = $(this).find(".Remark :input").val();
        var Volume = $(this).find(".Volume :input").val();
        var ExpCost = $(this).find(".ExpCost :input").val();
        var UnitId = $(this).find('.tdNone .TblCostHead').val();
        var Isactive = $(this).find('.tdNone .TblIsActive').val();
        var SurveyId = $(this).find('.tdNone .TblSurvey').val();
        var SurveyDetailId = $(this).find('.tdNone .TblSurveyDetail').val();
        var suveyerId = $('#SurveyerID').length > 0 ? $('#SurveyerID').val() : null;


        var alldata = {
            'SurveyDetailsID': SurveyDetailId,
            'SurveyID': SurveyId,
            'SurveyerID': null,
            'RateCompID': parseInt(RateCompID),
            'CostHeadID': parseInt(CostHeadID),
            'RemarksOnCostHead': Detail,
            'WtUnitID': parseInt(UnitId),
            'WtVolume': parseFloat(Volume),
            'ExpectedCost': parseFloat(ExpCost),
            'Isactive': parseFloat(Isactive)
        }
        data.push(alldata);
    });
    $(this).closest('#HFSOList').val(JSON.stringify({ 'SurveyDetail': data }));

    data = [];
    $(this).closest("#MoveCosttable > TBODY > tr").each(function () {
        //debugger;
        var CostHeadID = $(this).find('.tdNone .TblCostHead').val();
        var RateCompID = $(this).find('.tdNone .TblRateComp').val();
        var BaseCurrId = $(this).find('.tdNone .TblBaseCurr').val();
        var RateCurrId = $(this).find('.tdNone .TblRateCurr').val();
        var ConversionRate = $(this).find(".ConversionRate :input").val();
        var RevRateCurrId = CompanyID == '2' ? $(this).find('.tdNone .TblRevRateCurr').val() : $(this).find('.tdNone .TblRateCurr').val();
        var RevConversionRate = CompanyID == '2' ? $(this).find(".RevConversionRate :input").val() : $(this).find(".ConversionRate :input").val();
        var CostValue = $(this).find(".CostValue :input").val();
        var RevenueValue = $(this).find(".RevenueValue :input").val();
        //btnCostValue = btnCostValue + CostValue; btnRevenueValue = btnRevenueValue + RevenueValue;
        var Volume = $(this).find(".WtVol").html();
        var Per = $(this).find(".Per").html();
        var Rate = $(this).find(".Rate").html();
        var ExpCost = $(this).find(".ExpCost :input").val();
        var UnitId = $(this).find('.tdNone .TblCostHead').val();
        var Isactive = $(this).find('.tdNone .TblIsActive').val();
        var alldata = {
            'MoveCompID': parseInt(RateCompID),
            'CostHeadID': parseInt(CostHeadID),
            'BaseCurrID': parseInt(BaseCurrId),
            'RateCurrID': parseInt(RateCurrId),
            'ReRateCurrID': parseInt(RevRateCurrId),
            'WtUnitID': parseInt(UnitId),
            'Wt_Vol_No': parseFloat(Volume),
            'Per': parseInt(Per),
            'Rate': parseFloat(Rate),
            'ConversionRate': parseFloat(ConversionRate),
            'RevConversionRate': parseFloat(RevConversionRate),
            'CostValue': parseFloat(CostValue),
            'RevenueValue': parseFloat(RevenueValue),
            'Isactive': Isactive,
        }
        data.push(alldata);
    });

    $(this).closest("#CostDetails").find('#HFCostList').val(JSON.stringify({ 'CostHeadwiseDetail': data }));
}
function GetNotSurveyCostHeadXML(btnRevenueValue, btnCostValue, button, data, CompanyID) {

    $(this).closest("#SODetails").find("#MoveSOtable > TBODY > tr").each(function () {
        //
        var CostHeadID = $(this).find('.tdNone .TblCostHead').val();
        var RateCompID = $(this).find('.tdNone .TblRateComp').val();
        var Detail = $(this).find(".Remark :input").val();
        var Volume = $(this).find(".Volume :input").val();
        //var ExpCost = $(this).find(".ExpCost :input").val();
        var UnitId = $(this).find('.tdNone .TblCostHead').val();
        var Isactive = $(this).find('.tdNone .TblIsActive').val();
        var alldata = {
            'MoveCompID': parseInt(RateCompID),
            'CostHeadID': parseInt(CostHeadID),
            'ServOrderRemarks': Detail,
            'WtUnitID': parseInt(UnitId),
            'WtVolume': parseFloat(Volume),
            //'ExpectedCost': parseFloat(ExpCost),
            'Isactive': parseFloat(Isactive)
        }

        data.push(alldata);
    });
    $(this).closest("#SODetails").find('#HFSOList').val(JSON.stringify({ 'CostHeadwiseDetail': data }));

    data = [];
    //debugger;
    $(this).closest("#CostDetails").find("#MoveCosttable > TBODY > tr").each(function () {
        //
        var CostHeadID = $(this).find('.tdNone .TblCostHead').val();
        var RateCompID = $(this).find('.tdNone .TblRateComp').val();
        var BaseCurrId = $(this).find('.tdNone .TblBaseCurr').val();
        var RateCurrId = $(this).find('.tdNone .TblRateCurr').val();
        debugger;
        var RevRateCurrId = CompanyID == '2' ? $(this).find('.tdNone .TblRevRateCurr').val() : $(this).find('.tdNone .TblRateCurr').val();
        var ConversionRate = $(this).find(".ConversionRate :input").val();
        var RevConversionRate = CompanyID == '2' ? $(this).find(".RevConversionRate :input").val() : $(this).find(".ConversionRate :input").val();
        var CostValue = $(this).find(".CostValue :input").val();
        var RevenueValue = $(this).find(".RevenueValue :input").val();
        var Volume = $(this).find(".WtVol").html();
        var Per = $(this).find(".Per").html();
        var Rate = $(this).find(".Rate").html();
        var ExpCost = $(this).find(".ExpCost :input").val();
        var UnitId = $(this).find('.tdNone .TblCostHead').val();
        var Isactive = $(this).find('.tdNone .TblIsActive').val();
        var chkToBill = $(this).find('.tdNone .chkToBill').is(":checked");
        alldata = {
            'MoveCompID': parseInt(RateCompID),
            'CostHeadID': parseInt(CostHeadID),
            'BaseCurrID': parseInt(BaseCurrId),
            'RateCurrID': parseInt(RateCurrId),
            'RevRateCurrID': parseInt(RevRateCurrId),
            'WtUnitID': parseInt(UnitId),
            'Wt_Vol_No': parseFloat(Volume),
            'Per': parseInt(Per),
            'Rate': parseFloat(Rate),
            'ConversionRate': parseFloat(ConversionRate),
            'RevConversionRate': parseFloat(RevConversionRate),
            'CostValue': parseFloat(CostValue),
            'RevenueValue': parseFloat(RevenueValue),
            'ToBill': chkToBill,
            'Isactive': Isactive,
        }

        data.push(alldata);
    });
    $(this).closest("#CostDetails").find('#HFCostList').val(JSON.stringify({ 'CostHeadwiseDetail': data }));
}

function FreightValid(MoveID, data) {
    data = [];
    $("#MoveTranshiptable > TBODY > tr").each(function () {
		/*var CostHeadID = $(this).find('.tdNone .TblScheduleVessel').val();
		var RateCompID = $(this).find('.tdNone .TblETD').val();
		var BaseCurrId = $(this).find('.tdNone .TblETA').val();
		var RateCurrId = $(this).find('.tdNone .TblRateCurr').val();*/
        var ScheduleVessel = $(this).find(".ScheduleVessel :input").val();
        var ETD = $(this).find(".ETD :input").val();
        var ETA = $(this).find(".ETA :input").val();
        var Grd_Dt_ATD = $(this).find(".Grd_Dt_ATD").val() != null && $(this).find(".Grd_Dt_ATD").val() != "" ? $(this).find(".Grd_Dt_ATD").val() : null;
        var Grd_Dt_ATA = $(this).find(".Grd_Dt_ATA").val() != null && $(this).find(".Grd_Dt_ATA").val() != "" ? $(this).find(".Grd_Dt_ATA").val() : null;
        var TranshipPort = $(this).find(".TranshipPort select");
        var TranshipActive = $(this).find(".TblIsActive").val();
        var OrderNo = $(this).find(".Grd_OrderNo").val();
        var alldata = {
            'TransitPortID': parseInt(TranshipPort.val()),
            'SchVessel': ScheduleVessel,
            'EDD': ETD,
            'EDA': ETA,
            'ActDD': Grd_Dt_ATD,
            'ActDA': Grd_Dt_ATA,
            'Isactive': $.parseJSON(TranshipActive.toLowerCase()),
            'OrderNo': parseInt(OrderNo)
        }

        data.push(alldata);
    });
    $('#HFTransitList').val(JSON.stringify({ 'TransitInfo': data }));

    var InvJobRowCount = $('#TranshipInvJobTable > TBODY > tr').length;
    if (($('#MoveJob_FromLocationID option:selected').text().toLowerCase().indexOf("india") != -1 || $('#MoveJob_ToLocationID option:selected').text().toLowerCase().indexOf("india") != -1) && $('#ForwardingBr option:selected').val() == "") {
        alert('Forwarding Branch is required');
        event.preventDefault();
        return false;
    }
    else {

        var InvTotal = 0.00;
        var InvJobTotal = 0.00;
        var InvRowCount = $('#TranshipInvoiceTable > TBODY > tr').length;
        var InvJobRowCount = $('#TranshipInvJobTable > TBODY > tr').length;


        if (InvRowCount > 0 && InvJobRowCount <= 0) {

            alert('Invoice Amount distribution job required');
            event.preventDefault();
            return false;
        }
        else if (InvJobRowCount > 0) ////InvRowCount > 0 &&
        {

            var IsCurrentMoveID = $("#TranshipInvJobTable > TBODY > tr").filter(function () {
                return $(this).find('.TransitJob_MoveId').val() == MoveID;
            });

            if (IsCurrentMoveID.length == 0) {

                alert('Add Current Job No. in Invoice Amount distribution jobs List');
                event.preventDefault();
                return false;
            }
            else {

                $('#TranshipInvJobTable > TBODY > tr').each(function () {
                    var value = parseFloat($('.TransitJob_Amt', this).val());
                    if (!isNaN(value)) {
                        InvJobTotal += value;
                    }
                });

                $('#TranshipInvoiceTable > TBODY > tr').each(function () {
                    var value = parseFloat($('.Transit_InvoiceAmt', this).val());
                    if (!isNaN(value)) {
                        InvTotal += value;
                    }
                });

                if (InvTotal != InvJobTotal) {

                    alert('Invoice Total Amount not matching with distribution Jobs Total Amount');
                    event.preventDefault();
                    return false;
                }

            }
        }

    }

    $('#overlay').fadeIn();
}

function PackingValid(Packdate, OrgStgStartDate, OrgStgEndDate, ModeName, end, ISDeliveryDateValid) {
    var PackDate = $('#PackingInfo').find('#PackDate').val();
    var OrgStgStartDate = $('#PackingInfo').find('#OrgStgStartDate').val();
    var OrgStgEndDate = $('#PackingInfo').find('#OrgStgEndDate').val();
    var NetVol = parseFloat($('#PackingReport_NetVol').val());
    var GrossVol = parseFloat($('#PackingReport_GrossVol').val());
    var NwtWt = parseFloat($('#PackingReport_NetWt').val());
    var GrossWt = parseFloat($('#PackingReport_GrossWt').val());

    var start = new Date(PackDate);
    var Difference_In_Time = end.getTime() - start.getTime();
    var Difference_In_Days = Difference_In_Time / (1000 * 3600 * 24);

    var varPackDateValue = new Date(Packdate);
    if (varPackDateValue.getTime() != start.getTime()) {
        if (ISDeliveryDateValid == 'N' && Difference_In_Days > 7) {
            alert('Pack Date cannot be less than 7 days. Please inform your manager to update the same.');
            $('#overlay').fadeOut();
            event.preventDefault();
            return 'false';
        }
    }



    start = new Date(OrgStgStartDate);
    var Difference_In_Time = end.getTime() - start.getTime();
    var Difference_In_Days = Difference_In_Time / (1000 * 3600 * 24);
    if (new Date(OrgStgStartDate).getTime() != start.getTime()) {
        if (ISDeliveryDateValid == 'N' && Difference_In_Days > 5) {
            alert('Stg Start Date(Org) cannot be less than 5 days. Please inform your manager to update the same.');
            $('#overlay').fadeOut();
            event.preventDefault();
            return 'false';
        }
    }

    start = new Date(OrgStgEndDate);
    var Difference_In_Time = end.getTime() - start.getTime();
    var Difference_In_Days = Difference_In_Time / (1000 * 3600 * 24);
    if (new Date(OrgStgEndDate).getTime() != start.getTime()) {
        if (ISDeliveryDateValid == 'N' && Difference_In_Days > 5) {
            alert('Stg End Date(Org) cannot be less than 5 days. Please inform your manager to update the same.');
            $('#overlay').fadeOut();
            event.preventDefault();
            return 'false';
        }
    }

    if ((PackDate) && !(NetVol > 0 || GrossVol > 0 || NwtWt > 0 || GrossWt > 0)) {
        alert('Volume/Weight is required.');
        $('#overlay').fadeOut();
        event.preventDefault();
        return 'false';
    }
    var shptype = $('#MoveJob_ShipmentTypeID').val();
    var orgAgent = $('#PackingDetail_OrgAgentID').val();
    var destAgent = $('#PackingDetail_DestAgentID').val();

    if (ModeName == 'Road') {
        if (orgAgent == '') {
            alert('Please select Origin Agent.');
            $('#overlay').fadeOut();
            event.preventDefault();
            return 'false';
        }
    }
    else {
        if (shptype == '1' && (orgAgent == '' || destAgent == '')) {
            alert('Please select Origin and Destination Agents.');
            $('#overlay').fadeOut();
            event.preventDefault();
            return 'false';
        }
        else if (shptype == '2' && orgAgent == '') {
            alert('Please select Origin Agent.');
            $('#overlay').fadeOut();
            event.preventDefault();
            return 'false';
        }
        else if (shptype == '3' && destAgent == '') {
            alert('Please select Destination Agent.');
            $('#overlay').fadeOut();
            event.preventDefault();
            return 'false';
        }
    }



}

function DeliveryValid(MoveID, DeliveryDate, DestStgStartDate, DestStgEndDate, RMCBuss, Project, IsGCCInsurance, CompanyID, ModeName, button, end, ISDeliveryDateValid) {
    var DelDate = $('#DeliveryDate').val();
    var DestStgStartDate = $('#DeliveryInfo').find('#DestStgStartDate').val();
    var DestStgEndDate = $('#DeliveryInfo').find('#DestStgEndDate').val();
    var NetVol = parseFloat($('#DeliveryReport_NetVol').val());
    var GrossVol = parseFloat($('#DeliveryReport_GrossVol').val());
    var NwtWt = parseFloat($('#DeliveryReport_NetWt').val());
    var GrossWt = parseFloat($('#DeliveryReport_GrossWt').val());
    if (DelDate && !(NetVol > 0 || GrossVol > 0 || NwtWt > 0 || GrossWt > 0)) {
        alert('Volume/Weight is required.');
        $('#overlay').fadeOut();
        event.preventDefault();
        return 'false';
    }
    var start = new Date(DelDate);
    var Difference_In_Time = end.getTime() - start.getTime();
    var Difference_In_Days = Difference_In_Time / (1000 * 3600 * 24);
    //debugger;
    if (new Date(DeliveryDate).getTime() != start.getTime()) {
        if (ISDeliveryDateValid == 'N' && Difference_In_Days > 5) {
            alert('Delivery Date cannot be less than 5 days. Please inform your manager to update the same.');
            $('#overlay').fadeOut();
            event.preventDefault();
            return 'false';
        }
    }

    start = new Date(DestStgStartDate);
    var Difference_In_Time = end.getTime() - start.getTime();
    var Difference_In_Days = Difference_In_Time / (1000 * 3600 * 24);
    if (new Date(DestStgStartDate).getTime() != start.getTime()) {
        if (ISDeliveryDateValid == 'N' && Difference_In_Days > 5) {
            alert('Stg Start Date(Dest) cannot be less than 5 days. Please inform your manager to update the same.');
            $('#overlay').fadeOut();
            event.preventDefault();
            return 'false';
        }
    }

    start = new Date(DestStgEndDate);
    var Difference_In_Time = end.getTime() - start.getTime();
    var Difference_In_Days = Difference_In_Time / (1000 * 3600 * 24);
    if (new Date(DestStgEndDate).getTime() != start.getTime()) {
        if (ISDeliveryDateValid == 'N' && Difference_In_Days > 5) {
            alert('Stg End Date(Dest) cannot be less than 5 days. Please inform your manager to update the same.');
            $('#overlay').fadeOut();
            event.preventDefault();
            return 'false';
        }
    }

    if ((RMCBuss.toUpperCase() == "TRUE" || Project == "EXP" || Project == "IMP" || Project == "WCM") && CompanyID != '2') {
        if ($('#PassportNo').length > 0) {
            passportno = $('#PassportNo').val().trim();
        }
        else {
            passportno = $('#PassportNo').val().trim();
        }
        if (passportno.length == 0) {
            alert('Passport No is required');
            $('#overlay').fadeOut();
            event.preventDefault();
            return 'false';
        }

    }

    if ((!(CompanyID == '1' || CompanyID == '2' || CompanyID == '1005') && IsGCCInsurance.toUpperCase() == 'FALSE')
        && $('.InsurBy').val() == 1) {
        alert('Please save Insurance Details.');
        $('#overlay').fadeOut();
        event.preventDefault();
        return 'false';
    }

    var shptype = $('#MoveJob_ShipmentTypeID').val();
    var orgAgent = $('#DeliveryDetail_OrgAgentID').val();
    var destAgent = $('#DeliveryDetail_DestAgentID').val();

    if (ModeName == 'Road') {
        if (orgAgent == '') {
            alert('Please select Origin Agent.');
            $('#overlay').fadeOut();
            event.preventDefault();
            return 'false';
        }
    }
    else {
        if (shptype == '1' && (orgAgent == '' || destAgent == '')) {
            alert('Please select Origin and Destination Agents.');
            $('#overlay').fadeOut();
            event.preventDefault();
            return 'false';
        }
        else if (shptype == '2' && orgAgent == '') {
            alert('Please select Origin Agent.');
            $('#overlay').fadeOut();
            event.preventDefault();
            return 'false';
        }
        else if (shptype == '3' && destAgent == '') {
            alert('Please select Destination Agent.');
            $('#overlay').fadeOut();
            event.preventDefault();
            return 'false';
        }

    }
}

function fn_GPApproval(button, MoveID, Convurl) {
    GetGPAmount(button, Convurl, parseFloat($('#GPTotalRevenue').val()), MoveID, 'False');
}

function GetGPAmount(button, control, RevAmt, MoveID, IsGPApproved) {

    //if ($('#IsGPDirectSave').val() == 0) {
    //    $('#overlay').fadeOut();
    //    event.preventDefault();
    //}

    $.get(control, function (data) {
        debugger;
        $('#DefaultGPPercent').val(data.GPPercent);
        //$('#GPAmount').val(data.GPAmount);

        $(".GPMasterID").each(function () { $(this).val(data.GPMasterID); });


        var MasterGPPercent = parseFloat($('#DefaultGPPercent').val());
        //debugger;

        var GPPercent = $('#GPPercent').val();//(parseFloat(parseFloat($('#GPTotalRevenue').val()) - parseFloat($('#GPTotalCost').val())) / parseFloat($('#GPTotalRevenue').val())) * 100;

        //if (button == "btnSendToCSApprove" || button == "btnApproveDelivery") {
        //    var MasterGPPercent = parseFloat($('#DefaultGPPercent').val());
        //    //var GPPercent = (parseFloat(parseFloat($('#GPTotalRevenue').val()) - parseFloat($('#GPTotalCost').val())) / parseFloat($('#GPTotalRevenue').val())) * 100;

        //    if (MasterGPPercent > GPPercent && parseFloat(data.GPAmount) > parseFloat($('#GPTotalRevenue').val()) && IsGPApproved.toUpperCase() == 'FALSE') {
        //        alert('GP is Low. Please send for GP approval');
        //        $('#overlay').fadeOut();
        //        event.preventDefault();
        //        return 'false';
        //    }
        //    else {
        //        $('#IsGPDirectSave').val(1);
        //        $('#' + button).click();
        //    }


        //}
        //else {

        if (MasterGPPercent > GPPercent && parseFloat(data.GPAmount) > parseFloat($('#GPTotalRevenue').val())
            && $('#IsGPDirectSave').val() == 0) {
            if (button == "btnSaveSurvey"
                || button == "btnSaveDelivery"
                || button == "btnSavePacking") {
                //debugger;
                $('#overlay').fadeOut();
                event.preventDefault();
                swal({
                    title: "The GP Percent of costsheet is low. Do you want to send the costsheet for GP approval?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Send for Approval",
                    cancelButtonText: "No",
                    closeOnConfirm: true,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            //$('#GPSendForApproval').val(true);
                            debugger;
                            $('#IsGPDirectSave').val('1');
                            if (button == "btnSavePacking") {
                                $(".GPSendForApproval").each(function () { $(this).val(true); });
                                $(".GPPercent").each(function () { $(this).val(GPPercent); });
                                $(".GPAmount").each(function () { $(this).val($('#GPTotalRevenue').val()) });
                                $('#IsGPDirectSave').val("1");
                                //$('#BtnSubmit').val(button);
                                //$('#mdlGPPApproval').modal();
                                $('#btnSavePacking').click();
                            }
                            else if (button == "btnSaveDelivery") {
                                $(".GPSendForApproval").each(function () { $(this).val(true); });
                                $(".GPPercent").each(function () { $(this).val(GPPercent); });
                                $(".GPAmount").each(function () { $(this).val($('#GPTotalRevenue').val()) });
                                $('#IsGPDirectSave').val("1");
                                //$('#BtnSubmit').val(button);
                                //$('#mdlGPPApproval').modal();
                                $('#btnSaveDelivery').click();
                            }
                            else if (button == "btnSaveSurvey") {
                                $(".GPSendForApproval").each(function () { $(this).val(true); });
                                $(".GPPercent").each(function () { $(this).val(GPPercent); });
                                $(".GPAmount").each(function () { $(this).val($('#GPTotalRevenue').val()) });
                                $('#IsGPDirectSave').val("1");
                                //$('#BtnSubmit').val(button);
                                //$('#mdlGPPApproval').modal();
                                $('#btnSaveSurvey').click();
                            }
                        }
                        else {
                            //$('#IsGPDirectSave').val("1");
                            if (button == "btnSavePacking") {

                                //$('#btnSavePacking').click();
                                //$(".IsGPApproved").each(function () { $(this).val(0); });

                            }
                            else if (button == "btnSaveDelivery") {

                                //$('#btnSaveDelivery').click();
                                //$(".IsGPApproved").each(function () { $(this).val(0); });
                            }
                            else if (button == "btnSaveSurvey") {

                                //$('#btnSaveSurvey').click();
                                //$(".IsGPApproved").each(function () { $(this).val(0); });
                            }
                        }
                    });
            }

        }
        else {
            //if ($('#IsGPDirectSave').val() == 0) {
            //	$('#IsGPDirectSave').val("1");
            //	if (button == "btnSavePacking") {
            //		$('#btnSavePacking').click();
            //	}
            //	else if (button == "btnSaveDelivery") {
            //		$('#btnSaveDelivery').click();
            //	}
            //	else if (button == "btnSaveSurvey") {
            //		$('#btnSaveSurvey').click();

            //	}
            //}

        }
        //}

    });
}

function BtnExportCS(RMCName, ModeName, ShipperFName, ShipperLName, FromLocationName, OrgAgentName, ToLocationName, DestAgentName, url) {
    var tab_text = "<table border='2px'><tr >";
    debugger;
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: url,
        async: 'false',
        dataType: 'json',
        type: "GET",
        success: function (data) {

            if (parseInt(data.ColCount) > 0) {
                tab_text = tab_text + "<tr style='border: 0px'><td colspan='" + data.ColCount + "' bgcolor='#B0C4DE'><b>RMC : " + RMCName + "</b></td></tr>";
                tab_text = tab_text + "<tr><td colspan='" + data.ColCount + "' bgcolor='#B0C4DE'><b>Mode :" + ModeName + "</b></td></tr>";
                tab_text = tab_text + "<tr><td colspan='" + data.ColCount + "' bgcolor='#B0C4DE'><b>Shipper : " + ShipperFName + " " + ShipperLName + "</b></td></tr>";
                tab_text = tab_text + "<tr><td colspan='" + data.ColCount + "' bgcolor='#B0C4DE'><b>Origin : " + FromLocationName + "</b></td></tr>";
                tab_text = tab_text + "<tr><td colspan='" + data.ColCount + "' bgcolor='#B0C4DE'><b>Origin Agent : " + OrgAgentName + "</b ></td ></tr > ";
                tab_text = tab_text + "<tr><td colspan='" + data.ColCount + "' bgcolor='#B0C4DE'><b>Destination : " + ToLocationName + "</b></td></tr>";
                tab_text = tab_text + "<tr><td colspan='" + data.ColCount + "' bgcolor='#B0C4DE'><b>Destination Agent : " + DestAgentName + "</b></td></tr>";
            }

            tab_text = tab_text + data.htmlstring;
            tab_text = tab_text.replace(/<tr>/g, "");
            //tab_text = tab_text.replace(/_([^]*)$/, '$1'); //a_bc

            tab_text = tab_text + "</table>";
            tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, "");//remove if u want links in your table
            tab_text = tab_text.replace(/<img[^>]*>/gi, ""); // remove if u want images in your table
            tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, ""); // reomves input params

            var ua = window.navigator.userAgent;
            var msie = ua.indexOf("MSIE ");

            if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))      // If Internet Explorer
            {
                txtArea1.document.open("txt/html", "replace");
                txtArea1.document.write(tab_text);
                txtArea1.document.close();
                txtArea1.focus();
                sa = txtArea1.document.execCommand("SaveAs", true, "CostSheet.xls");
            }
            else                 //other browser not tested on IE 11
                sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(tab_text));

            return (sa);
        },
        error: function (reponse) {
            alert("error : " + reponse);
        }
    });

    var textRange; var j = 0;
}

function ShipperFeedback_SetHiddenFieldValue() {
    var strData = "";
    if (SFQuestionIDList.length > 0)
        strData = { 'SFQuestionIDList': SFQuestionIDList };

    if (strData)
        $("#SFQuestionIDListHidden").val(JSON.stringify(strData));
    else
        $("#SFQuestionIDListHidden").val(null);
}

function BindQuestionList() {
    if ($("#tableSFQuestions tbody tr.trQuestions").length > 0) {
        $("#tableSFQuestions tbody tr.trQuestions").each(function () {
            if ($(this).find(".IsQuestionChecked").is(":checked")) {
                var SFQuestionID = parseInt($(this).find(".SFQuestionID").val());
                var SFAnswerOptionID = null;
                var AnswerText = null;

                var SFQuestionIDObj = {
                    'SFQuestionID': SFQuestionID
                }
                if (SFAnswerOptionID !== null) SFQuestionIDObj.SFAnswerOptionID = SFAnswerOptionID;
                if (AnswerText !== null) SFQuestionIDObj.AnswerText = AnswerText;

                SFQuestionIDList.push(SFQuestionIDObj);
            }
        });
    }
    ShipperFeedback_SetHiddenFieldValue();
}

function TemplateValidationRules() {

    //Add validation rule for dynamically generated AnswerText fields
    $('.AnswerText').each(function () {
        $(this).rules("add",
            {
                required: true,
                messages: {
                    required: "Remark is required"
                }
            });
    });
    //Add validation rule for dynamically generated AnswerDate fields
    $('.AnswerDate').each(function () {
        $(this).rules("add",
            {
                required: true,
                messages: {
                    required: "Date is required"

                }
            });
    });

    //Add validation rule for dynamically generated answerList fields
    $('.answerList').each(function () {
        $(this).rules("add",
            {
                required: true,
                messages: {
                    required: "Select one option"

                }
            });
    });

    //$('.answerRadioBtn').each(function () {
    //    $(this).rules('add', {
    //        require_from_group: [1, $(this)],
    //        messages: {
    //            required: "Select one option"

    //        }
    //    });
    //});


    $("input:radio[class*='answerRadioBtn_']").each(function () {

        //debugger;
        var ChckClass = $(this).attr('id');
        $('#' + ChckClass).rules("add", {
            required: function (elem) {
                return $("input." + ChckClass + ":checked").length <= 0;
            },
            messages: {
                required: "Select at least one"
            }
        });

    });

    $("input:checkbox[class^='answerchkOptions_']").each(function () {

        //debugger;
        var ChckClass = $(this).attr('id');
        $('#' + ChckClass).rules("add", {
            required: function (elem) {
                return $("input." + ChckClass + ":checked").length <= 0;
            },
            messages: {
                required: "Select at least one"
            }
        });

    });
}

function CopyAddress(sender) {


    var id = $(sender).attr('id');
    //console.log($(sender).parent('#popover-content-Origin'));
    $(sender).parent().parent().mask("Loading...");

    //ShiperAddress
    var shipAdd1 = $('#Address1').val();
    var shipAdd2 = $('#Address2').val();
    var shipCity = $('#City').val();
    //alert(shipCity);
    var shipEmail = $('#Email').val();
    var shipPin = ''//$('#').val();
    var shipPhone = $('#Phone1').val();

    var Addr1 = id == 'CopyOrigin' ? $('#popOrgAdd') : $('#popDestAdd');
    var Addr2 = id == 'CopyOrigin' ? $('#popOrgAdd2') : $('#popDestAdd2');
    var City = id == 'CopyOrigin' ? $("#PopOrgCity") : $("#PopDestCity");
    var pin = id == 'CopyOrigin' ? $('#PopOrgPin') : $('#PopDestPin');
    var Email = id == 'CopyOrigin' ? $('#popOrgEmail') : $('#popOrgEmail');
    var Phone = id == 'CopyOrigin' ? $('#popOrgPhone') : $('#popOrgPhone');

    //$.get($(sender).attr('data-href'), function (content, status) {


    //if (content != null && content.Address != null && content.Address.length>=6) {
    //
    Addr1.val(shipAdd1);
    Addr2.val(shipAdd2);
    City.val(shipCity).attr('selected', 'selected');
    pin.val(shipPin);
    Phone.val(shipPhone);
    Email.val(shipEmail);

    //.preventDefault();
    $(sender).parent().parent().unmask();
    $(sender).parents(".popover").popover('hide');
    // }

    //return false;

    //    });

    return false;

}

function Set_ModalFollowUp() {
    //
    $('#FollowUp_FollowUpDate').val($('#dtFollowUpDate').val());
    $('#FollowUp_FollowUpRemark').val($('#txtFollowUpRemark').val());
}

function Set_ModalJobClose() {
    //
    //$('#FollowUp_FollowUpDate').val($('#dtFollowUpDate').val());
    $('#CloseJobRemark').val($('#txtJobCloseRemark').val());
}

function Set_ModalACODetails() {
    $('#ACODetails_JobStatusSDId').val($('#ddl_JobStatusSDId').val());
    $('#ACODetails_BillingStatusId').val($('#ddl_BillingStatusId').val());
    $('#ACODetails_Remarks').val($('#txtRemarks').val());
}

function Set_ModalCancelJob() {
    //
    //$('#FollowUp_FollowUpDate').val($('#dtFollowUpDate').val());
    $('#JobCancel_CancelRemark').val($('#txtCancelRemark').val());
}

function Set_ModalInsuranceDetail(IsSendForInsurance) {
    //
    $('#Insurance_InsPackDate').val($('#dtInsPackDate').val());
    $('#Insurance_InsuranceValue').val($('#txtInnsuranceValue').val());
    $('#Insurance_IDVCarValue').val($('#txtIDVCarValue').val());
    $('#Insurance_PremiumRate').val($('#txtPremiumRate').val());
    $('#Insurance_VehMakeModel').val($('#txtVehMakeModel').val());
    $('#Insurance_InsuranceCurrID').val($('#ddl_InsuranceCurr').val());
    $('#Insurance_IsSendForInsurance').val(IsSendForInsurance);
}

function Set_ModalGCCInsuranceDetail(IsSendForInsurance) {
    //
    $('#GCCinsuranceDetail_PolicyNumber').val($('#txtPolicyNumber').val());
    $('#GCCinsuranceDetail_Remark').val($('#txtRemarks').val());
    $('#Insurance_IDVCarValue').val($('#txtIDVCarValue').val());
    $('#Insurance_PremiumRate').val($('#txtPremiumRate').val());
    $('#Insurance_VehMakeModel').val($('#txtVehMakeModel').val());
    $('#Insurance_InsuranceCurrID').val($('#ddl_InsuranceCurr').val());
    $('#Insurance_IsSendForInsurance').val(IsSendForInsurance);
}

function GetShipperDetails(flag) {
    //alert($('#Title').val());
    var value;
    if (flag == 'get') {

        value = {
            title: $('#Title').val(),
            Fname: $('#ShipperFName').val(),
            Lname: $('#ShipperLName').val(),
            ShipperType: $('#ShipperType').val(),
            //City: $('#')
            Email: $('#Email').val(),
            Address1: $('#Address1').val(),
            Address2: $('#Address2').val(),
            Phone1: $('#Phone1').val(),
            Phone2: $('#Phone2').val(),
        };

    }
    if (flag == 'set') {
        value = JSON.parse($('#BillData').val());
        $('#Title').val(value.title == undefined ? 'Mr' : value.title);
        $('#ShipperFName').val(value.Fname);
        $('#ShipperLName').val(value.Lname);
        $('#ShipperType').val(value.ShipperType);
        $('#Email').val(value.Email);
        $('#Address1').val(value.Address1);
        $('#Address2').val(value.Address2);
        $('#Phone1').val(value.Phone1);
        $('#Phone2').val(value.Phone2);
    }

    return value;
}

/////Total Cost Calculation for Quote
function TotalCalCostEstimateDetail() {

    var NetAmount = 0;
    var OrgAmt = 0;
    var FrgAmt = 0;
    var DestAmt = 0;
    var BaseEstimate = 0;

    var OrgQutAmt = 0;
    var FrgQutAmt = 0;
    var DestQutAmt = 0;
    var NeQutAmt = 0;

    $('#CostHeadtableDetail > TBODY > tr').each(function () {

        var RateCompDropdownText = $('.tdRateComponent', this).text().trim();
        var tdAmount = (isNaN(parseFloat($('.tdAmount', this).text().trim()))) ? 0 : parseFloat($('.tdAmount', this).text().trim());
        var tdTotalAmount = (isNaN(parseFloat($('.tdTotalAmount', this).text().trim()))) ? 0 : parseFloat($('.tdTotalAmount', this).text().trim());
        BaseEstimate = tdAmount;
        NetAmount = NetAmount + BaseEstimate;
        NeQutAmt = NeQutAmt + tdTotalAmount;

        if (RateCompDropdownText.toLowerCase() == 'origin') {

            OrgAmt = OrgAmt + BaseEstimate;
            OrgQutAmt = OrgQutAmt + tdTotalAmount;

            $('#txtOriginEstimateDetail').val(OrgAmt);
            $('#txtOriginQuoteDetail').val(OrgQutAmt);
        }
        else if (RateCompDropdownText.toLowerCase() == 'freight') {

            FrgAmt = FrgAmt + BaseEstimate;
            FrgQutAmt = FrgQutAmt + tdTotalAmount;

            $('#txtFrightEstimateDetail').val(FrgAmt);
            $('#txtFrightQuoteDetail').val(FrgQutAmt);
        }
        else if (RateCompDropdownText.toLowerCase() == 'destination') {

            DestAmt = DestAmt + BaseEstimate;
            DestQutAmt = DestQutAmt + tdTotalAmount;
            $('#txtDestinationEstimateDetail').val(DestAmt);
            $('#txtDestinationQuoteDetail').val(FrgQutAmt);

        }
    });


    $('#txtNetEstimateDetail').val(NetAmount);
    $('#txtNetQuoteDetail').val(NeQutAmt);
}

function TotalCalEstimate() {

    var NetAmount = 0;
    var OrgAmt = 0;
    var FrgAmt = 0;
    var DestAmt = 0;
    var GrossAmount = 0;
    var BaseEstimate = 0;

    if ($('#MoveCostHeadtable').length) {

        $('#MoveCostHeadtable > TBODY > tr').each(function () {

            var RateCompDropdownText = $('.RateComponent', this).text().trim();

            TbltxtConversionRate = (isNaN(parseFloat($('.TbltxtConversionRate', this).val()))) ? 0 : parseFloat($('.TbltxtConversionRate', this).val())
            TbltxtAmount = (isNaN(parseFloat($('.TbltxtAmount', this).val()))) ? 0 : parseFloat($('.TbltxtAmount', this).val())

            BaseEstimate = TbltxtAmount * TbltxtConversionRate;
            if (!isNaN(BaseEstimate)) {

                $('.BaseEstimate', this).html(BaseEstimate);

            }
            else {

                $('.BaseEstimate', this).html("0");
                BaseEstimate = 0;
            }

            GrossAmount = GrossAmount + BaseEstimate;

            if (RateCompDropdownText.toLowerCase() == 'origin') {

                OrgAmt = OrgAmt + BaseEstimate;

                $('#txtOriginEstimate').val(OrgAmt);
            }
            else if (RateCompDropdownText.toLowerCase() == 'freight') {

                FrgAmt = FrgAmt + BaseEstimate;
                $('#txtFrightEstimate').val(FrgAmt);
            }
            else if (RateCompDropdownText.toLowerCase() == 'destination') {

                DestAmt = DestAmt + BaseEstimate;
                $('#txtDestinationEstimate').val(DestAmt);

            }
        });

        $("#txtNetBaseEst").val(GrossAmount);

    }

}

function getInstructionList(button) {
    var data = [];

    if (button == "btnSaveJob") {
        $('#MoveRateComponenttable tbody tr').each(function () {

            var RateCompID = $(this).find('.TblRateComponent').val();
            var AgentID = $(this).find('.TblHFVJobAgent').val();
            var TransFrom = $(this).find('.TbltxtTransitTimeF').val();
            var TransTo = $(this).find('.TbltxtTransitTimeT').val();
            var alldata = {
                'CompID': parseInt(RateCompID),
                'AgentID': parseInt(AgentID),
                'TransTimeFrom': TransFrom,
                'TransTimeTo': TransTo
            }
            data.push(alldata);


        });
    }



    var pair = { 'CostHeadwiseDetail': data }
    return pair;
}

function TimePickerClass() {
    $('#SurveyTime').datetimepicker({
        format: 'H:m',
    });
}



function BillPrepared(control, controlid, IsStatement, url) {
    $.get(url, function (data) {
        //console.log('a');

        if (data.errormsg != null && data.errormsg != '') {
            toastr.error(data.errormsg);
        }
        else {

            var modal_id = "#mdlBillPrepareDelivery"; //+ controlid.replace('btn','mdl');
            var formcontrol = control.closest('.row');
            if (data.InvoiceList.length > 1) {
                $(modal_id + ' .InvoiceList option').remove();
                $.each(data.InvoiceList, function (i) {
                    $(modal_id + ' .InvoiceList').append($('<option></option>').val(data.InvoiceList[i].Value).html(data.InvoiceList[i].Text));
                });
                $(modal_id + ' .InvoiceList').trigger('change');

                if (controlid == 'btnStatementCharges') {
                    $(modal_id + ' #btnProceed').addClass('hide');
                    $('#ShowStatementCharges').removeClass('hide');
                    $(modal_id + " .InvoiceList option[html='New Invoice']").remove();
                }
                else {
                    $(modal_id + ' #btnProceed').removeClass('hide');
                    $('#ShowStatementCharges').addClass('hide');
                }
                $(modal_id).modal();
            }
            else if (data.InvoiceList.length == 1) {
                $(modal_id + ' .InvoiceList option').remove();
                $.each(data.InvoiceList, function (i) {
                    $(modal_id + ' .InvoiceList').append($('<option></option>').val(data.InvoiceList[i].Value).html(data.InvoiceList[i].Text));
                    $(modal_id + ' .InvoiceList').val(data.InvoiceList[i].Value);
                    formcontrol.find('#InvoiceID').val(data.InvoiceList[i].Value);
                });
                //$('#Form_Packing').submit();
                ////
                if (controlid == 'btnStatementCharges') {

                    $(modal_id).modal();
                    //alert("No Invoice allcated to this job.");
                    $(modal_id + ' #btnProceed').addClass('hide');
                    $('#ShowStatementCharges').removeClass('hide');
                }
                else {

                    $(modal_id + ' #btnProceed').trigger('click');
                    $(modal_id + ' #btnProceed').removeClass('hide');
                    $('#ShowStatementCharges').addClass('hide');

                }
            }
            //else if (data.InvoiceList.length <= 0)
            //{
            //    alert("No Invoice allcated to this job.");
            //}
        }
    });
}

function Datepicker(dt) {
    dt.datetimepicker({
        format: "DD-MMM-YYYY",

        widgetPositioning: {
            horizontal: 'right',
            vertical: 'bottom'
        }
    })
        .on('hide', function () {
            if (!this.firstHide) {
                if (!$(this).is(":focus")) {
                    this.firstHide = true;
                    // this will inadvertently call show (we're trying to hide!)
                    this.focus();
                }
            } else {
                this.firstHide = false;
            }
        })
        .on('show', function () {
            if (this.firstHide) {
                // careful, we have an infinite loop!
                $(this).datepicker('hide');
            }
        })

}

function htmlEntities(str) {
    return str.includes('<') ?
        String(str).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;')
        : String(str).replace(/&amp;/g, '&').replace(/&lt;/g, '<').replace(/&gt;/g, '>').replace(/&quot;/g, '"');
}

function chkFunction(control) {

    if ($(control).is(':checked')) {
        $(control).siblings('.chksibling').removeClass('hide');
    }
    else {
        $(control).siblings('.chksibling').addClass('hide');
    }
}

function DownloadTransShipment(sender) {

    var btn = $(sender);
    var url = btn.attr('data-url');

    if (url != null && url != "") {

        var win = window.open(url, '_blank');
        if (win) {
            //Browser has allowed it to be opened
            win.focus();
        } else {
            //Browser has blocked it
            alert('Please allow popups for this website');
        }
    }

    return false;
}

function fn_IsDirectHideShow(control, LCLFCL, MoveID) {
    if (LCLFCL == 'FCL') {
        $('.divisDirect').removeClass('hide');
    }
    else {
        $('.divisDirect').addClass('hide');
    }

    if (LCLFCL == 'LCL') {

        $('#FCLinput').addClass('hide');
    }
    else {

        $('#FCLinput').removeClass('hide');
    }

    debugger;

    if ((LCLFCL != 'GRPG' && $('#dll_TransitDistJobNo').val() != MoveID) || $('#dll_TransitDistJobNo').val() == null || $('#dll_TransitDistJobNo').val() == "") {

        $('#dll_TransitDistJobNo').val(MoveID).trigger('change');
    }

}

function ClearTranship(element) {
    //var element = $(button).closest('.divTransShip');
    element.find("#txt_ScheduleVessel").val(null);

    element.find("#Dt_ETD").val(null);
    element.find("#Dt_ETA").val(null);
    element.find("#Dt_ATD").val(null);
    element.find("#Dt_ATA").val(null);

    element.find(".ddl_TranshipPortId").val('').attr('selected', 'selected');
}

function IsTranshipment(control) {

    var istranship = $(control).is(':checked');
    var element = $(control).closest('.divTransShip').siblings('.TranshipTable');
    //$(element).('')
    //(control).closest('.densfact').siblings('.TranshipTable');
    rowCount = element.find('#MoveTranshiptable > TBODY > tr').find('.TblIsActive')
        .filter(function (index) {
            // function needs to return a boolean.
            return $(this).val() == 'true';
        }).length;

    if (!istranship && rowCount > 1) {
        alert('You can only proceed with one transhipment in direct. Please delete other.');
        $(control).prop("checked", true);
        $('#btnAddTranShipment').removeClass('disabled');
    }
    else if (!istranship && rowCount == 1) {
        $('#btnAddTranShipment').addClass('disabled');
    }
    else {
        $('#btnAddTranShipment').removeClass('disabled');
    }

}

function TranshipRemove(button) {
    var closeTr = $(button).closest('tr');
    //var BatchId = btn.find('.tdNone').find('.TblBatch').val();
    var table = $(button).closest('.TranshipTable');
    var previousRow = closeTr.prev();
    var CurrentRowToPort = closeTr.find('.Grd_Dll_TranshipPortId').val();
    if (confirm('Are you sure you want to delete this?')) {


        //var Flag = closeTr.find('.tdNone').find('.TblIsActive').hasClass('saved');
        //if (Flag) {

        //    closeTr.find('.tdNone').find('.TblIsActive').val('false');
        //    closeTr.hide();
        //}
        //else
        //{
        closeTr.remove();
        if (previousRow != null) {
            previousRow.find('.Grd_Dll_TranshipPortId').val(CurrentRowToPort);///Update the previous row to port
            previousRow.find('.btnTrasitWtRemove').removeClass('hide');
        }
        //}
        var IsActiveRowContains = false;
        table.find('.TblIsActive').each(function () {

            if ($(this).val() == "true") {
                IsActiveRowContains = true;
            }
        });

        var element = $('.divTransShip');
        var TransitShip = element.find('#TransitShipment').is(':checked');

        if (IsActiveRowContains && !TransitShip) {
            $('#btnAddTranShipment').addClass('disabled');
        }
        else {
            $('#btnAddTranShipment').removeClass('disabled');
        }
    }
    //if (table.length >= 0 && table.length == 1) {

    //}
    GetTranshipList(table);
}

function GetTranshipList(table) {
    var data = [];

    table.find("#MoveTranshiptable > TBODY > tr").each(function (index, el) {

		/*var CostHeadID = $(this).find('.tdNone .TblScheduleVessel').val();
		var RateCompID = $(this).find('.tdNone .TblETD').val();
		var BaseCurrId = $(this).find('.tdNone .TblETA').val();
		var RateCurrId = $(this).find('.tdNone .TblRateCurr').val();*/
        var ScheduleVessel = $(this).find(".ScheduleVessel :input").val();
        var ETD = $(this).find(".ETD :input").val();
        var ETA = $(this).find(".ETA :input").val();
        var Grd_Dt_ATD = $(this).find(".Grd_Dt_ATD").val() != null && $(this).find(".Grd_Dt_ATD").val() != "" ? $(this).find(".Grd_Dt_ATD").val() : null;
        var Grd_Dt_ATA = $(this).find(".Grd_Dt_ATA").val() != null && $(this).find(".Grd_Dt_ATA").val() != "" ? $(this).find(".Grd_Dt_ATA").val() : null;
        var TranshipPort = $(this).find(".TranshipPort select");
        var TranshipActive = $(this).find(".TblIsActive").val();

        if (TranshipActive != null && $.parseJSON(TranshipActive.toLowerCase())) {
            var Order = index + 1;
            $(this).find(".Grd_OrderNo").val(Order);
        }
        var OrderNo = $(this).find(".Grd_OrderNo").val();

        var alldata = {
            'TransitPortID': parseInt(TranshipPort.val()),
            'SchVessel': ScheduleVessel,
            'EDD': ETD,
            'EDA': ETA,
            'ActDD': Grd_Dt_ATD,
            'ActDA': Grd_Dt_ATA,
            'Isactive': TranshipActive != null ? $.parseJSON(TranshipActive.toLowerCase()) : true,
            'OrderNo': parseInt(OrderNo)

        }

        data.push(alldata);
    });
    table.siblings('#HFTransitList').val(JSON.stringify({ 'TransitInfo': data }));

    ////Tooltip on Vessel

    $('.Grd_txt_ScheduleVessel').bind('input', function () {
        $(this).attr('title', $(this).val());
    });
}

function datepickerInit_Tranship() {
    $('.datepicker').datetimepicker({
        format: "DD-MMM-YYYY",
        useCurrent: false,
        widgetPositioning: {
            horizontal: 'right',
            vertical: 'bottom'
        }
    });

    $('.Grd_Dt_ETA,.Grd_Dt_ETD,.Grd_Dt_ATD,.Grd_Dt_ATA').datetimepicker().on('dp.show', function () {
        $(this).closest('.table-responsive').removeClass('table-responsive').addClass('temp');
    });
    $('.Grd_Dt_ETA,.Grd_Dt_ETD,.Grd_Dt_ATD,.Grd_Dt_ATA').datetimepicker().on('dp.hide', function () {
        $(this).closest('.temp').addClass('table-responsive').removeClass('temp')
    });

    // $('.GrdEnableselect').select2();
}

function fn_TranshipAdd(button, Mode) {
    var element = $(button).closest('.divTransShip');
    var txtScheduleVessel = element.find("#txt_ScheduleVessel").val();
    var DtETD = element.find("#Dt_ETD").val();
    var DtETA = element.find("#Dt_ETA").val();
    var DtATD = element.find("#Dt_ATD").val();
    var DtATA = element.find("#Dt_ATA").val();

    var TranshipPortDropdown = element.find(".ddl_TranshipPortId");
    var TranshipPortDropdownSelected = TranshipPortDropdown.val();
    var TranshipPortDropdownText = element.find(".ddl_TranshipPortId :selected").text();
    var FromPort = $('#PortLoad option:selected').val();
    var ToPort = $('#PortDischarge option:selected').val();
    var rowCount = 0;
    //clsFlag = table.siblings('#HFFlag').val();
    //var txtRevenueValue = $("#txt_RevenueValue").val();
    /// Get the Agent from the MoveJob Table
    var table = element.siblings('.TranshipTable');
    var VesselLable = "Schedule Vessel";
    if (Mode == 'Road') {
        VesselLable = "Transporter Name";
    }
    else if (Mode == 'Air') {
        VesselLable = "Flight No";
    }

    var tblRowCount = table.find('TBODY > tr').length;

    if ((txtScheduleVessel == null || txtScheduleVessel == "") || (DtETD == null || DtETD == "") || (DtETA == null || DtETA == "")) {

        alert(VesselLable + ', ETD & ETA required');
        return false;
    }

    if (Mode != 'Road') {

        if (tblRowCount <= 0 && (isNaN(parseInt(FromPort)) || isNaN(parseInt(ToPort)))) {

            alert('Port Of Load & Port Of Discharge  required');
            return false;
        }

        if (tblRowCount > 0 && (isNaN(parseInt(TranshipPortDropdownSelected)))) {

            alert('TranshipPort required');
            return false;
        }

        if (tblRowCount > 0 && !isNaN(parseInt(TranshipPortDropdownSelected)) && ValidateTransitPortNotExsist(TranshipPortDropdownSelected)) {

            alert('Selected Tranship Port already added');
            return false;
        }

    }

    if (ValidatePreviousETAwithCurrentETD(DtETD)) {

        alert('ETD should not be less than previous ETA');
        return false;
    }

    if (true
        //CostHeadDropdownSelected > 0 && RateCurrDropdownSelected > 0 && BaseCurrDropdownSelected > 0
        //    && txtCostValue != null && txtCostValue != "" && txtConversionRate != null && txtConversionRate != ""
    ) {
        var breakOut = false;
        table.find('.btnTrasitWtRemove').addClass('hide');
        var tBody = table.find("#MoveTranshiptable > TBODY")[0];

        rowCount = table.find('#MoveTranshiptable > TBODY > tr').length;

        var tdHFVCostHead = '<input type="hidden" id="HFVIsActive_' + (rowCount + 1) + '" class="TblIsActive" value="true" />'
            + '<input type="hidden" class="Grd_OrderNo" id="HFVOrderNo_' + (rowCount + 1) + '" value="1" />';

        //   + '<input type="hidden" id="HFVBaseCurrID_' + (rowCount + 1) + '" class="TblBaseCurr" value="' + BaseCurrDropdownSelected + '" />'
        //    + '<input type="hidden" id="HFVRateCurrID_' + (rowCount + 1) + '" class="TblRateCurr" value="' + RateCurrDropdownSelected + '" />'
        //    + '<input type="hidden" id="HFVRateCompId_' + (rowCount + 1) + '" class="TblRateComp" value="' + RateCompDropdownSelected + '" />'
        //var tdHFVCostHead = '<input type="hidden" id="HFVTranshipPortID_' + (rowCount + 1) + '" class="TblTranshipPort" value="" />
        //    + txtServiceInstructValue + '</textarea>';
        var tdHFVScheduleVessel = '<input type="text" name="Grd_txt_ScheduleVessel" class="form-control input-sm clearfix Grd_txt_ScheduleVessel" id="Grd_txt_ScheduleVessel_' + (rowCount + 1) + '" value="' + txtScheduleVessel + '" title="' + txtScheduleVessel + '" />'
                /*+ '<a href = "javascript:void(0)" id="Grd_txt_ScheduleVessel_tooltip' + (rowCount + 1) + '" class="fa fa-info-circle Grd_txt_ScheduleVessel_tooltip" role="button"></a>'*/;

        var tdHFVETD = '<div class="input-group input-group-sm date" id="dtp_ToDate">' +
            '<input class="form-control input-sm datepicker text-box single-line Grd_Dt_ETD" id="Dt_ETD_' + (rowCount + 1) + '" type="text" value="' + DtETD + '" autocomplete="off">' +
            '<span class="input-group-addon" style="display: none">' +
            '<span class="glyphicon glyphicon-calendar"></span>' +
            '</span></div>';
        var tdHFVETA = '<div class="input-group input-group-sm date" id="dtp_ToDate">' +
            '<input class="form-control input-sm datepicker text-box single-line Grd_Dt_ETA" id="Dt_ETA_' + (rowCount + 1) + '" type="text" value="' + DtETA + '" autocomplete="off">' +
            '<span class="input-group-addon" style="display: none">' +
            '<span class="glyphicon glyphicon-calendar"></span>' +
            '</span></div>';
        var tdHFVATD = '<div class="input-group input-group-sm date" id="dtp_ToDate">' +
            '<input class="form-control input-sm datepicker text-box single-line Grd_Dt_ATD" id="Dt_ATD_' + (rowCount + 1) + '" type="text" value="' + DtATD + '" autocomplete="off">' +
            '<span class="input-group-addon" style="display: none">' +
            '<span class="glyphicon glyphicon-calendar"></span>' +
            '</span></div>';
        var tdHFVATA = '<div class="input-group input-group-sm date" id="dtp_ToDate">' +
            '<input class="form-control input-sm datepicker text-box single-line Grd_Dt_ATA" id="Dt_ATA_' + (rowCount + 1) + '" type="text" value="' + DtATA + '" autocomplete="off">' +
            '<span class="input-group-addon" style="display: none">' +
            '<span class="glyphicon glyphicon-calendar"></span>' +
            '</span></div>';
        var option = null;

        $('#ddl_TranshipPortDemo').find('option').each(function (data) {
            option = option + '<option value="' + $(this).val() + '">' + $(this).html() + '</option>'
        });
        var tdHFVTranshipPort = '<select class="form-control selectdropdown Grd_Dll_TranshipPortId GrdEnableselect" id="Grd_ddl_TranshipPortId_' + (rowCount + 1) + '"  onchange="TranshipPortChange(this)">' + option + '</select>';
        var tdFromPort = '<select class="form-control selectdropdown Grd_Dll_FromPortId GrdEnableselect" id="Grd_ddl_FromPortId_' + (rowCount + 1) + '">' + option + '</select>';

        //Add Row.
        var row = tBody.insertRow(-1);
        row.className = "package-row";

        //Add CostHeadID cell.
        var cell = $(row.insertCell(-1));
        cell.html(tdHFVCostHead);
        cell.addClass("tdNone");

        //Add RateComp cell.
        //Add RateComp cell.


        cell = $(row.insertCell(-1));
        cell.html(tdHFVScheduleVessel);
        cell.addClass("ScheduleVessel");

        //1 - From Port
        //2 - ETD
        //3 - Act Dept
        //4 - To Port
        //5 - ETA
        //6 - Act Arr


        var hideFromToPort = Mode == "Road" ? "hide" : "";

        cell = $(row.insertCell(-1));
        cell.html(tdFromPort);
        cell.addClass("From");
        cell.addClass(hideFromToPort);

        cell = $(row.insertCell(-1));
        cell.html(tdHFVETD);
        cell.addClass("ETD");

        //Add Agent cell.
        cell = $(row.insertCell(-1));
        cell.html(tdHFVATD);
        cell.addClass("ATD");

        cell = $(row.insertCell(-1));
        cell.html(tdHFVTranshipPort);
        cell.addClass("TranshipPort");
        cell.addClass(hideFromToPort);

        cell = $(row.insertCell(-1));
        cell.html(tdHFVETA);
        cell.addClass("ETA");

        //Add CostHeadText cell.
        cell = $(row.insertCell(-1));
        cell.html(tdHFVATA);
        cell.addClass("ATA");

        //Add Button cell.
        cell = $(row.insertCell(-1));
        var btnRemove = '<button type="button" class="btn btn-danger btn-sm btnTrasitWtRemove" onclick="TranshipRemove(this)"  id="btnRemove"><span class="glyphicon glyphicon-trash"></span></button>';
        //var btnEdit = '<a onclick="EditInst(this);" id="CostHeadDetails_' + (rowCount + 1) + '" data-id=' + (rowCount + 1) + ' class="btn btn-xs btn-default glyphicon glyphicon-pencil EditNone" data-cache="false" title="Detail CostHead" data - assigned - id=' + (rowCount + 1) + ' data-toggle="modal" data-target="#EditModal"></a>';
        cell.append(btnRemove);

        var prevrow = $(row).prev();
        if (rowCount == 0) {
            ToPort = (TranshipPortDropdownSelected == null || TranshipPortDropdownSelected == "") ? ToPort : TranshipPortDropdownSelected;
            $('#Grd_ddl_TranshipPortId_' + (rowCount + 1)).val(ToPort);
        }
        else {

            ToPort = prevrow.find('.Grd_Dll_TranshipPortId').val();
            prevrow.find('.Grd_Dll_TranshipPortId').val(TranshipPortDropdownSelected);
            $('#Grd_ddl_TranshipPortId_' + (rowCount + 1)).val(ToPort);
        }
        FromPort = (prevrow.find('.Grd_Dll_TranshipPortId').val() == null || prevrow.find('.Grd_Dll_TranshipPortId').val() == "") ? FromPort : prevrow.find('.Grd_Dll_TranshipPortId option:selected').val();
        $('#Grd_ddl_FromPortId_' + (rowCount + 1)).val(FromPort);

        GetTranshipList(table);

        var TransitShip = element.find('#TransitShipment').is(':checked');
        if ((rowCount + 1) == 1 && !TransitShip) {
            $(button).addClass('disabled');
        }
        ClearTranship(element);
        //Clear the TextBoxes.
        //CostHeadDropdown.prop("selected", "0");
        datepickerInit_Tranship();

        // $("#txt_CostValue").val("");

    }
    else {
        alert('Cost Head, Base Currency,Rate Currency,Conversion Rate and Cost Value are required');
        return false;
    }
}

function TranshipPortChange(sender) {

    var row = $(sender).closest('tr');
    var next = row.next();
    if (next != null) {

        next.find('.Grd_Dll_FromPortId').val(row.find('.Grd_Dll_TranshipPortId').val());
    }

    return false;
}

function ValidateTransitPortNotExsist(PortID) {


    var result = false;
    $('#MoveTranshiptable > TBODY > tr').each(function () {

        var self = $(this);
        var PreviousPortID = self.find(".Grd_Dll_TranshipPortId option:selected").val();
        var IsActive = self.find('.TblIsActive').val();
        if (PreviousPortID != null && PortID != null && $.parseJSON(IsActive.toLowerCase()) && PreviousPortID == PortID) {

            result = true;
        }
    });

    return result;
}

function ValidatePreviousETAwithCurrentETD(ETD) {

    var result = false;
    $('#MoveTranshiptable > TBODY > tr').each(function () {

        var self = $(this);
        var PreviousETA = Date.parse(self.find(".Grd_Dt_ETA").val().trim());
        var CurrentETD = Date.parse(ETD);
        var IsActive = self.find('.TblIsActive').val();
        if (PreviousETA != null && CurrentETD != null && $.parseJSON(IsActive.toLowerCase()) && CurrentETD < PreviousETA) {

            result = true;
        }
    });

    return result;
}

function CalculateVolWt(element1, name, RMCType) {

    var element = $(element1);
    var parent = element.closest('.Wt_Vol');
    var id = element.attr('id');
    var name = name + "Report";
    var dens_fact = parent.find('#' + name + '_DensityFact').val();
    var LCLFCL = parent.find('#' + name + '_LCLorFCL').val();
    var LOOSECASED = parent.find('#' + name + '_LooseCased').val();
    var ship_type = $('#MoveJob_ModeID :selected').text();
    var UnitType = $('option:selected', parent.find('#' + name + '_WeightUnitID')).text();
    var VolUnitType = $('option:selected', parent.find('#' + name + '_VolumeUnitID')).text();
    var actcontrol = 0;
    var value = element.val();
    if (id == name + '_DensityFact' || id == name + '_LCLorFCL' || id == name + '_LooseCased') {
        actcontrol = 1;
        if (RMCType == 'Cartus Type') {
            id = (ship_type == 'Air' ? name + '_ACWTWt' : name + '_NetVol');
            value = $('#' + id).val();
        }
        else if (RMCType == 'Brookfield Type') {
            id = (ship_type == 'Air' ? name + '_GrossVol' : name + '_NetWt');
            value = $('#' + id).val();
        }
        else if (RMCType == 'Other Type') {
            id = (ship_type == 'Air' ? name + '_NetVol' : name + '_NetVol');
            value = $('#' + id).val();
        }
    }

    var control;
    //if (id == name + '_Mode' || id == name + '_DensityFact') {

    switch (id) {
        case name + '_TobePackedVol':
            control = 'VOL_TO_PACK';
            break;
        case name + '_NetVol':
            control = 'VOL_NET';
            break;
        case name + '_GrossVol':
            control = 'VOL_GROSS';
            break;
        case name + '_ACWTWt':
            control = 'WT_ACWT';
            break;
        case name + '_NetWt':
            control = 'WT_NET';
            break;
        case name + '_GrossWt':
            control = 'WT_GROSS';
            break;
    }

    var result = WtVol_Calculation(control, value, dens_fact, ship_type, UnitType, VolUnitID, LCLFCL, LOOSECASED, actcontrol);
    //console.log(result);
    parent.find('#' + name + '_TobePackedVol').val(parseFloat(result.topack || 0).toFixed(2));
    parent.find('#' + name + '_NetVol').val(parseFloat(result.volnet || 0).toFixed(2));
    parent.find('#' + name + '_GrossVol').val(parseFloat(result.volgross || 0).toFixed(2));
    parent.find('#' + name + '_ACWTWt').val(parseFloat(result.acwt || 0).toFixed(2));
    parent.find('#' + name + '_NetWt').val(parseFloat(result.wtnet || 0).toFixed(2));
    parent.find('#' + name + '_GrossWt').val(parseFloat(result.wtgross || 0).toFixed(2));
    if (result.errmsg) {
        parent.find('#' + name + '_DensityFact').focus();
    }
}

function UnitChange(element1) {

    var element = $(element1);
    var parent = element.closest('.Wt_Vol');
    var unit = $('option:selected', element).text();
    var unitfrom, control, value;
    if (element.attr('id') == name + '_VolumeUnitId') {
        unitfrom = unit == 'CBM' ? 'CFT' : 'CBM';
        //alert(unit + '-' + unitfrom);
        control = parent.find('#' + name + '_VolumeToPack');
        value = parseFloat(control.val());
        control.val(parseFloat(Unit(value, null, unitfrom, unit) || 0).toFixed(2));
        control = parent.find('#' + name + '_VolumeNet');
        value = parseFloat(control.val());
        control.val(parseFloat(Unit(value, null, unitfrom, unit) || 0).toFixed(2));
        control = parent.find('#' + name + '_VolumeGross');
        value = parseFloat(control.val());
        control.val(parseFloat(Unit(value, null, unitfrom, unit) || 0).toFixed(2));
    }
    else {
        unitfrom = unit == 'KG' ? 'LBS' : 'KG';
        control = parent.find('#' + name + '_WtACWT');
        control.val(parseFloat(Unit(parseFloat(control.val()), null, unitfrom, unit) || 0).toFixed(2));
        control = parent.find('#' + name + '_WtNet');
        control.val(parseFloat(Unit(parseFloat(control.val()), null, unitfrom, unit) || 0).toFixed(2));
        control = parent.find('#' + name + '_WtGross');
        control.val(parseFloat(Unit(parseFloat(control.val()), null, unitfrom, unit) || 0).toFixed(2));
    }
}

function WtVolValidation(element1, name) {
    var element = $(element1);
    var parent = element.closest('.Wt_Vol');
    var id = element.attr('id');
    var name = name + "Report";
    var LCLFCL = parent.find('#' + name + '_LCLorFCL').val();
    var LOOSECASED = parent.find('#' + name + '_LooseCased').val();

    if (!LOOSECASED) {
        //errmsg = "Density factor, Loose/Cased and LCL/FCL are required to calculate weight/volume.";
        errmsg = "Loose/Cased are required to enter weight/volume.";
        element.val(0.00);
        alert(errmsg);
        parent.find('#' + name + '_LooseCased').focus();
        return false;
    }
    if (!LCLFCL) {
        errmsg = "LCL/FCL are required to enter weight/volume.";
        element.val(0.00);
        alert(errmsg);
        parent.find('#' + name + '_LCLorFCL').focus();
        return false;
    }
}

function DestinationTHC_Change(sender) {

    $(".DestinationTHC").not(sender).prop('checked', false);
}

function ShowDatePicker(date) {
    var dt = $(date);
    //Datepicker(dt);
    $(date).siblings('.input-group-addon').click();
}

function fn_loadTransitJobNo(ControlId, Masterid, Mode, url) {
    var selectdepartment = $(ControlId);
    //$('#dll_TransitInvJobNo');
    $.getJSON(url, null, function (data) {

        selectdepartment.html('').select2({ data: [{ id: '', text: '' }] });
        if (selectdepartment.prop('multiple') != true) {
            var Default = new Option('Select Job No', '', true, true);
            selectdepartment.append(Default);
        }
        //$(selectdepartment).append('<option value=0>Please Select</option>');
        for (var i = 0; i < data.CountryList.length; i++) {
            var newState = new Option(data.CountryList[i].Text, data.CountryList[i].Value);
            // Append it to the select
            selectdepartment.append(newState);
        }
        selectdepartment.trigger('change');
    });
    GetTrasitInvoiceTotal();
    GetTrasitInvJobTotal();
}

function fn_InstructionValid(MoveId, Cheque_No, Cheque_Amt, url) {
    $.getJSON(url, null, function (data) {

        if (data.result == true && (Cheque_No == null || Cheque_No == "" || Cheque_Amt == null || Cheque_Amt == "")) {

            swal({
                title: "",
                text: "Advance or Pre Payment related details needs to be captured in Job before requesting for WIS Instructions as this is a Private shipper OR advance collection  OR Pre payment instructions  is mentioned on the B&C for this shipment",
                icon: "warning",
            });
            return false;
        }

        $("#InstructionSheetForm").submit();

    });
}

function fn_DatePickerClass(DeliveryDateValue, PackDateValue, SurveyDateValue) {
    $('.datepicker').datetimepicker({
        format: "DD-MMM-YYYY",
        useCurrent: false,
        widgetPositioning: {
            horizontal: 'right',
            vertical: 'bottom'
        },
    });
    $('.DeliveryDate,.PackDate,.SurveyDate').datetimepicker({
        format: "DD-MMM-YYYY",
        widgetPositioning: {
            horizontal: 'right',
            vertical: 'bottom'
        },
        maxDate: new Date()
    });

    $('.DeliveryDate').val(DeliveryDateValue);
    $('.PackDate').val(PackDateValue);
    $('.SurveyDate').val(SurveyDateValue);

    $('.Grd_Dt_ETA,.Grd_Dt_ETD,.Grd_Dt_ATD,.Grd_Dt_ATA').datetimepicker().on('dp.show', function () {
        $(this).closest('.table-responsive').removeClass('table-responsive').addClass('temp');
    });
    $('.Grd_Dt_ETA,.Grd_Dt_ETD,.Grd_Dt_ATD,.Grd_Dt_ATA').datetimepicker().on('dp.hide', function () {
        $(this).closest('.temp').addClass('table-responsive').removeClass('temp')
    });


    ////Create Tooltip on Vessel

    $('.Grd_txt_ScheduleVessel').bind('input', function () {
        $(this).attr('title', $(this).val());
    });
}

////DocUploadPartial

////DocUploadPartial Close

////TranshipInvoicePartial

////TranshipInvoicePartial Close

////RateComponentPartial

////RateComponentPartial

////SO Partial

//// SO Partial

//// TranshipWtVolPartial

//// TranshipWtVolPartial

//// RequestDocs

//// RequestDocs

//// RequestDocsUpload

//// RequestDocsUpload Close

////Shipper FeedBack

//// Shipper Feedback Close

////Scripts

$('.summernote').summernote();

$("body").on("change", "#ReportID", function () {

    if ($(this).find("option:selected").val() != null && $(this).find("option:selected").val() != "") {
        $("#hfReportName").val($(this).find("option:selected").text());
        document.forms["FormReport"].submit();
    }

});

$("textarea").on('blur', function () {
    $(this).attr('title', $(this).val());
});



$('.InvoiceList').on('change', function () {
    ////
    var invid = $(this).attr('selected', 'selected').val();
            /*$(this).closest('.modal').find*/$('#InvoiceID').val(invid);
});

$('#PackingReport,#DelReport').collapse('show');

$('.CollapsingFieldSet').on('hidden.bs.collapse', toggleIcon);

$('.CollapsingFieldSet').on('shown.bs.collapse', toggleIcon);

function toggleIcon(e) {
    $(e.target).closest('fieldset').toggleClass('border-top border-bottom');
    $(e.target)
        .prev('legend')
        .find(".more-less")
        .toggleClass('glyphicon-plus glyphicon-minus');

}

$(document).on("click", ".popover .close", function () {
    $(this).parents(".popover").popover('hide');
});

$('#btnSendToMobile').click(function (event) {

    if ($('#SurveyerID').val() == null || $('#SurveyerID').val() == "") {

        alert("Surveyor required");
        event.preventDefault();
    }
    else {
        $('#overlay').fadeIn();
    }

});

////To change the height of the model to the screen size -200
$(".modal-wide").on("show.bs.modal", function () {
    var height = $(window).height() - 20;
    $(this).find(".modal-body").css("max-height", height);
});

$('#btnSendToApprove,#btnPackSendToApprove,#btnSurveySendToApprove').on('click', function () {

    var count = $('#SendtoApprovalList option').length;
    var control = $(this).attr('id');
    var ddlControl = $('#PackingReport_CSSenttoApproveUser');
    if (control == 'btnSendToApprove') {
        $('#btnSendToCSApprove').attr('form', 'Form_Delivery');
        ddlControl = $('#CSSenttoApproveUser');
    }
    else if (control == 'btnPackSendToApprove') {
        $('#btnSendToCSApprove').attr('form', 'Form_Packing');
    }
    else if (control == 'btnSurveySendToApprove') {
        $('#btnSendToCSApprove').attr('form', 'Form_Survey');
        ddlControl = $('#SurveyReport_CSSenttoApproveUser');
    }
    if (count > 1) {
        $('#mdlSendtoApproval').modal();
    }
    else {
        //////debugger;
        $(ddlControl).val($('#SendtoApprovalList').val());

        $('#btnSendToCSApprove').click();
    }

});

$('#SendtoApprovalList').on('change', function () {
    if ($('#btnSendToCSApprove').attr('form') == 'Form_Packing') {
        $('#PackingReport_CSSenttoApproveUser').val($('#SendtoApprovalList').val());
    }
    else if ($('#btnSendToCSApprove').attr('form') == 'Form_Delivery') {
        $('#CSSenttoApproveUser').val($('#SendtoApprovalList').val());
    }
    else if ($('#btnSendToCSApprove').attr('form') == 'Form_Survey') {
        $('#SurveyReport_CSSenttoApproveUser').val($('#SendtoApprovalList').val());
    }

});

$('#btnSendToCSApprove').click(function (e) {
    var ApprovalSelect = $('#CSSenttoApproveUser').val();
    if ($('#btnSendToCSApprove').attr('form') == 'Form_Packing') {
        ApprovalSelect = $('#PackingReport_CSSenttoApproveUser').val();
    }
    else if ($('#btnSendToCSApprove').attr('form') == 'Form_Survey') {
        ApprovalSelect = $('#SurveyReport_CSSenttoApproveUser').val();
    }
    else if ($('#btnSendToCSApprove').attr('form') == 'Form_Delivery') {
        ApprovalSelect = $('#CSSenttoApproveUser').val();
    }

    if (ApprovalSelect <= 0) {
        e.preventDefault();
        alert("Please select Approval User.");
    }
});

$("#btnDocSearch").click(function (e) {


    e.preventDefault();
    $("#DocTypeID").val($("#DocTypeDropdown").val());
    $("#DocNameID").val($("#DocNameDropdown").val());
    $("#DocDescription").val($("#jobDocUpload_DocDescription").val());
    $("form#DocCustomFilter").submit();
});