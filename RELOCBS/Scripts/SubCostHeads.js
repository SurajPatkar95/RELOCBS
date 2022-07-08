function GetSubCostHead(url, CostHeadID, RateComponentID)
{
	$.get(url, function (data) {
		$('#SubCostHead').append(data.SubCostHeadList);
		

		if (CostHeadID > 0 && RateComponentID > 0)
			PopUpSubCost(CostHeadID, RateComponentID, data.IsEdit);
	});
}

function IsSubCostHead(route) {
	//var route = ;
	//route = route.replace("-1", CostHeadID);
	var IsCostHead = false;
	$.ajax({
		type: "GET",
		url: route,
		async: false,
		error: function (data) {
			
			$('#IsSubCost').val(data.IsSubCost);
		},
		success: function (data) {
			
			$('#IsSubCost').val(data.IsSubCost);
			if (JSON.parse(data.IsSubCost)) {
				//GetSubCost(CostHeadID, RateComponentID);

			}
		},
	});
	
	//$('#partial').load(route, function () {
	//});
}
//$(document.re
function CalculateAmount(CostHeadID, RateCompID ,IsGrid) {
	//
	//var RateCompID = $(element).closest('.SubCost').find('.hfPopRateCompID').val();
	//var CostHeadID = $(element).closest('.SubCost').find('.hfPopCostHeadID').val();
	var SumCost = 0;
	$('#SubCostHead').find('.SubCost').each(
		function () {
			var hfRateCompID = $(this).find('.hfPopRateCompID').val();
			var hfCostHeadID = $(this).find('.hfPopCostHeadID').val();
			if (RateCompID == hfRateCompID && CostHeadID == hfCostHeadID) {
				SumCost = SumCost + parseInt($(this).find('.txtvalue').val());
			}
		}); 
	//isGrid -> 0= gridedit, 1= costheadedit
	//alert(IsGrid);
	if (IsGrid == '0') {
		$('#CostHeadtable > TBODY > tr').each(function () {
			var TDCostHeadID = $(this).find('td .TblHFVCostHead').val();
			var TDRateCompID = $(this).find('td .TblRateComponent').val();
			
			if (TDCostHeadID == CostHeadID && TDRateCompID == RateCompID) {
				$(this).find('td .TbltxtAmount').val(SumCost);
				var convRate = $(this).find('td .TbltxtConversionRate').val();
				$(this).find('td.BaseEstimate').html(convRate * SumCost);
			}
		});
	}
	else {
		$('#txtAmount').val(SumCost);
	}
	getAllSubCostData();
	//TotalCalEstimate()
}

function getAllSubCostData() {
	var data = [];
	$('#SubCostHead').find('.SubCost').each(function () {
		
		var hfRateCompID = $(this).find('.hfPopRateCompID').val();
		var hfCostHeadID = $(this).find('.hfPopCostHeadID').val();
		var hfSubCostHeadID = $(this).find('.hfPopSubCostHeadID').val();
		var Value = $(this).find('.txtvalue').val();
		var rate = $(this).find('.txtrate').val();
		var convrate = $(this).find('.txtconvrate').val();
		var currid = $(this).find('.ddlcurr').val();

		//$(this).find('.ddlcurr > option').each(function () {
		//	
		//	if ($(this).html() == $(this).closest('select').val()) {
		//		currid = $(this).attr('value');
		//	}
		//});
		var alldata = {
			'CompID': parseInt(hfRateCompID),
			'CostHeadID': parseInt(hfCostHeadID),
			'SubCostHeadID': parseInt(hfSubCostHeadID),
			'RateCurrID': parseInt(currid),
			'RateValue': parseFloat(rate),
			'ConvRate': parseFloat(convrate),
			'Amt': parseFloat(Value)
		}
		data.push(alldata);
	});

	var pair = { 'SubCostDetail': data }
	//return pair;
	$('#HFSubCostList').val(JSON.stringify(pair));
}


function PopUpSubCost(CostHeadID, RateCompID, IsEdit) {

	$('#SubCostHead').find('.SubCost').each(
		function()
		{
			var hfRateCompID = $(this).find('.hfPopRateCompID').val();
			var hfCostHeadID = $(this).find('.hfPopCostHeadID').val();
			
			if (RateCompID == hfRateCompID && CostHeadID == hfCostHeadID) {
				$(this).removeClass('hide');
			}
			else {
				if (!$(this).hasClass('hide')) {
					$(this).addClass('hide');
				}
			}
		}
	);
	
	if (IsEdit == '0') {
		//$('.modal-footer').addClass('hide');
		$('#btnSubCostCancel').removeAttr('onclick', 'SubCostRemove(' + CostHeadID + ', ' + RateCompID + ');').addClass('hide');
		$('#btnSubCostAdd').attr('onclick', 'SubCostAdd(0,' + CostHeadID + ', ' + RateCompID + ')');
		$('.SubCostClose').removeClass('hide');
	}
	else {
		//$('.modal-footer').removeClass('hide');
		$('#btnSubCostAdd').attr('onclick', 'SubCostAdd(1,' + CostHeadID + ', ' + RateCompID + ')');
		$('#btnSubCostCancel').attr('onclick', 'SubCostRemove(' + CostHeadID + ', ' + RateCompID + ');').removeClass('hide');
		$('.SubCostClose').addClass('hide');
	}
	$('#SubCostHeadModal').modal('show');
}

function SubCostAdd(IsAdd, CostHeadID, RateCompID)
{
	
	$(this).find('.SubCost');
	//var hfRateCompID = $(element).find('.hfPopRateCompID').val();
	//var hfCostHeadID = $(element).find('.hfPopCostHeadID').val();
	
	CalculateAmount(CostHeadID, RateCompID, IsAdd)
	
	if (IsAdd == 1) {
		$('#btnAdd').trigger('click');
	}
	getAllSubCostData();
	$('#SubCostHeadModal').modal('hide');
}

function SubCostRemoveJS(url, CostHeadID, RateCompID) {
	$.get(url, function (data) {
		//$('#SubCostHead').append(data.SubCostHeadList);
		//

		//if (CostHeadID > 0 && RateComponentID > 0)
		//	PopUpSubCost(CostHeadID, RateComponentID, data.IsEdit);
	});
	$('#SubCostHead').find('.SubCost').each(function () {
		//
		//var element = $(this).find('.SubCost');
		var hfRateCompID = $(this).find('.hfPopRateCompID').val();
		var hfCostHeadID = $(this).find('.hfPopCostHeadID').val();
		if (RateCompID == hfRateCompID && CostHeadID == hfCostHeadID) {
			$(this).remove();
		}
	});
	$('#SubCostHeadModal').modal('hide');
	getAllSubCostData();
	
	$('#CostHeadDropdown').val('').change();
}
function GetCostHeadDetails()
{
}

