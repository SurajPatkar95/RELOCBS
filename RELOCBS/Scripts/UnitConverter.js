var ControlList = { 'VTP': 'VOL_TO_PACK', 'VN': 'VOL_NET', 'VG': 'VOL_GROSS', 'WN': 'WT_NET', 'WG': 'WT_GROSS', 'WA': 'WT_ACWT' };

function Unit(val, val2, fromunit, tounit) {
    switch (fromunit + "-" + tounit) {
        case "CBM-CFT":
            return val * 35.315;
            break;
        case "CFT-CBM":
            return val / 35.315;
            break;
        case "LBS-KG":
            return val / 2.20462;
            break;
        case "KG-LBS":
            return val * 2.20462;
            break;
        case "DIGIT-PERCENT":
            return (val * (val2 / 100));
            break;
            /*case "PERCENT-DIGIT":
                return (val * (val2 / 100));
                break;*/
    }
    //alert(val);

}

function WtVol_Calculation(control, value, dens_fact, ship_type, WtUnitType, VolUnitType, LCLFCL, LOOSECASED, actcontrol) {
    var IsSameGross = false;
    if (LOOSECASED == 'Cased') {
        IsSameGross = false;
    }
    else {
        IsSameGross = LCLFCL == 'LCL';
    }

    value = parseFloat(value);
    dens_fact = dens_fact ? parseFloat(dens_fact) : 0;
    var topack, volnet, volgross, wtnet, wtgross, acwt, const_fact;
    if (WtUnitType == 'LBS')
        const_fact = Unit(4.72, null, 'KG', 'LBS');
    else
        const_fact = 4.72;

    errmsg = null;
    var conValue = 20;
    var conPer = 1.2;
    var isLoose = (ship_type == "Sea" || ship_type == "Road") && LOOSECASED == "Loose";


    switch (control) {
        case "VOL_TO_PACK":
            if (dens_fact <= 0 || !dens_fact || !LOOSECASED || !LCLFCL) {
                if (actcontrol == 0) {
                    errmsg = "Density factor, Loose/Cased and LCL/FCL are required to calculate weight/volume.";
                    alert(errmsg);
                    break;
                }
            }
            topack = value;
            volnet = value + Unit(topack, 15, 'DIGIT', 'PERCENT');
            volgross = isLoose ? volnet : volnet + Unit(volnet, conValue, 'DIGIT', 'PERCENT');
            //volgross = volnet + (IsSameGross ? Unit(volnet, conValue, 'DIGIT', 'PERCENT') : 0);
            //volgross = volnet + Unit(volnet, Percent, 'DIGIT', 'PERCENT');
            wtnet = (VolUnitType == 'CBM' && ship_type == "Sea" ? Unit(volnet, null, 'CBM', 'CFT') : volnet) * dens_fact;
            ////wtnet = VolUnitType == 'KGS' ? wtnet / 2.2 : wtnet;
            wtgross = (VolUnitType == 'CBM' && ship_type == "Sea" ? Unit(volnet, null, 'CBM', 'CFT') : volgross) * dens_fact;
            ////wtgross = VolUnitType == 'KGS' ? wtgross / 2.2 : wtgross;


            wtnet = volnet * dens_fact;
            wtgross = volgross * dens_fact;
            acwt = ship_type == 'Air' ? wtgross : volgross * const_fact;
            break;

        case "VOL_NET":
            if (dens_fact <= 0 || !LOOSECASED || !LCLFCL) {
                if (actcontrol == 0) {
                    errmsg = "Density factor, Loose/Cased and LCL/FCL are required to calculate weight/volume.";
                    alert(errmsg);
                    break;
                }
            }
            volnet = value;
            topack = volnet / 1.15;//- Unit(volnet, 15, 'DIGIT', 'PERCENT');
            //volgross = volnet + Unit(volnet, 20, 'DIGIT', 'PERCENT');
            //volgross = volnet + (IsSameGross ? Unit(volnet, conValue, 'DIGIT', 'PERCENT') : 0);//+ Unit(volnet, Percent, 'DIGIT', 'PERCENT');
            volgross = isLoose ? volnet : volnet + Unit(volnet, conValue, 'DIGIT', 'PERCENT');
            wtnet = (VolUnitType == 'CBM' && ship_type == "Sea" ? Unit(volnet, null, 'CBM', 'CFT') : volnet) * dens_fact;
            //wtnet = VolUnitType == 'KGS' ? wtnet / 2.2 : wtnet;
            wtgross = (VolUnitType == 'CBM' && ship_type == "Sea" ? Unit(volnet, null, 'CBM', 'CFT') : volgross) * dens_fact;
            ////wtgross = VolUnitType == 'KGS' ? wtgross / 2.2 : wtgross;
            ////wtnet = volnet * dens_fact;
            wtgross = volgross * dens_fact;
            acwt = ship_type == 'Air' ? wtgross : volgross * const_fact;
            break;
        case "VOL_GROSS":
            if (dens_fact <= 0 || !LOOSECASED || !LCLFCL) {
                if (actcontrol == 0) {
                    errmsg = "Density factor, Loose/Cased and LCL/FCL are required to calculate weight/volume.";
                    alert(errmsg);
                    break;
                }
            }
            volgross = value;
            volnet = isLoose ? volgross : (volgross / conPer);
            //volnet = IsSameGross ? (volgross / 1.2) : volgross;//- Unit(volgross, 20, 'DIGIT', 'PERCENT');
            topack = volnet / 1.15;//- Unit(volnet, 15, 'DIGIT', 'PERCENT');
            wtnet = (VolUnitType == 'CBM' && ship_type == "Sea" ? Unit(volnet, null, 'CBM', 'CFT') : volnet) * dens_fact;
            //wtnet = VolUnitType == 'KGS' ? wtnet / 2.2 : wtnet;
            wtgross = (VolUnitType == 'CBM' && ship_type == "Sea" ? Unit(volnet, null, 'CBM', 'CFT') : volgross) * dens_fact;
            //wtgross = VolUnitType == 'KGS' ? wtgross / 2.2 : wtgross;
            ////wtnet = volnet * dens_fact;
            ////wtgross = volgross * dens_fact;
            acwt = ship_type == 'Air' ? wtgross : volgross * const_fact;
            break;
        case "WT_NET":
            if (dens_fact <= 0 || !LOOSECASED || !LCLFCL) {
                if (actcontrol == 0) {
                    errmsg = "Density factor, Loose/Cased and LCL/FCL are required to calculate weight/volume.";
                    alert(errmsg);
                    break;
                }
            }
            wtnet = value;
            volnet = wtnet / dens_fact;
            topack = volnet / 1.15;//- Unit(volnet, 15, 'DIGIT', 'PERCENT');
            volgross = isLoose ? volnet : volnet + Unit(volnet, conValue, 'DIGIT', 'PERCENT');
            //volgross = volnet + (IsSameGross ? Unit(volnet, conValue, 'DIGIT', 'PERCENT') : 0); //Unit(volnet, Percent, 'DIGIT', 'PERCENT');
            wtnet = (VolUnitType == 'CBM' && ship_type == "Sea" ? Unit(volnet, null, 'CBM', 'CFT') : volnet) * dens_fact;
            //wtnet = VolUnitType == 'KGS' ? wtnet / 2.2 : wtnet;
            wtgross = (VolUnitType == 'CBM' && ship_type == "Sea" ? Unit(volnet, null, 'CBM', 'CFT') : volgross) * dens_fact;
            //wtgross = VolUnitType == 'KGS' ? wtgross / 2.2 : wtgross;
            ////wtnet = volnet * dens_fact;
            ////wtgross = volgross * dens_fact;
            acwt = ship_type == 'Air' ? wtgross : volgross * const_fact;
            break;
        case "WT_GROSS":
            if (dens_fact <= 0 || !LOOSECASED || !LCLFCL) {
                if (actcontrol == 0) {
                    errmsg = "Density factor, Loose/Cased and LCL/FCL are required to calculate weight/volume.";
                    alert(errmsg);
                    break;
                }
            }
            wtgross = value;
            volgross = wtgross / dens_fact;
            volnet = isLoose ? volgross : (volgross / conPer);
            //volnet = IsSameGross ? (volgross / 1.2) : volgross;//- Unit(volgross, 20, 'DIGIT', 'PERCENT');
            topack = volnet / 1.15// - Unit(volnet, 15, 'DIGIT', 'PERCENT');
            wtnet = (VolUnitType == 'CBM' && ship_type == "Sea" ? Unit(volnet, null, 'CBM', 'CFT') : volnet) * dens_fact;
            //wtnet = VolUnitType == 'KGS' ? wtnet / 2.2 : wtnet;
            ////wtnet = volnet * dens_fact;
            acwt = ship_type == 'Air' ? wtgross : volgross * const_fact;
            break;
        case "WT_ACWT":
            if (dens_fact <= 0 || !LOOSECASED || !LCLFCL) {
                if (actcontrol == 0) {
                    errmsg = "Density factor, Loose/Cased and LCL/FCL are required to calculate weight/volume.";
                    alert(errmsg);
                    break;
                }
            }
            acwt = value;
            if (ship_type == 'Air') {
                wtgross = acwt;
                volgross = wtgross / dens_fact;
            }
            else {
                volgross = acwt / const_fact;
                wtgross = volgross * dens_fact;
            }
            volnet = volgross = isLoose ? volgross : (volgross / conPer);
            //volnet = IsSameGross ? (volgross / 1.2) : volgross;//volgross / 1.2;//- Unit(volgross, 20, 'DIGIT', 'PERCENT');
            topack = volnet / 1.15//- Unit(volnet, 15, 'DIGIT', 'PERCENT');
            wtnet = (VolUnitType == 'CBM' && ship_type == "Sea" ? Unit(volnet, null, 'CBM', 'CFT') : volnet) * dens_fact;
            //wtnet = VolUnitType == 'KGS' ? wtnet/2.2 :wtnet
            ////wtnet = volnet * dens_fact;
            break;
        default:
    }

    return result = { topack, volnet, volgross, wtnet, wtgross, acwt, errmsg };
}

//function WtVol_Calculation(control, value, dens_fact, ship_type, UnitType, LCLFCL, LOOSECASED, actcontrol)
//{
//	var IsSameGross = false;
//	if (LOOSECASED == 'Cased') {
//		IsSameGross = false;
//	}
//	else {
//		IsSameGross = LCLFCL == 'LCL';
//	}

//    value = parseFloat(value);
//	dens_fact = dens_fact ? parseFloat(dens_fact): 0;
//    var topack, volnet, volgross, wtnet, wtgross, acwt, const_fact;
//    if (UnitType == 'LBS')
//        const_fact = Unit(4.72, null, 'KGS', 'LBS');
//    else
//		const_fact = 4.72;

//	errmsg = null;


//	switch (control) {
//		case "VOL_TO_PACK":
//			if ((dens_fact <= 0 || !LOOSECASED || !LCLFCL) && actcontrol=0) {
//				errmsg = "Dencity factor, Loose/Cased and LCL/FCL are required to calculate weight/volume.";
//				alert(errmsg);
//				break;
//			}
//				topack = value;
//				volnet = value + Unit(topack, 15, 'DIGIT', 'PERCENT');
//				volgross = volnet + (IsSameGross ? Unit(volnet, 20, 'DIGIT', 'PERCENT') : 0);
//			volgross = volnet + Unit(volnet, Percent, 'DIGIT', 'PERCENT');
//				wtnet = volnet * dens_fact;
//				wtgross = volgross * dens_fact;
//				acwt = ship_type == 'Air' ? wtgross : volgross * const_fact;
//				break;

//		case "VOL_NET":
//			if ((dens_fact <= 0 || !LOOSECASED || !LCLFCL) && actcontrol= 0) {
//				errmsg = "Dencity factor, Loose/Cased and LCL/FCL are required to calculate weight/volume.";
//				alert(errmsg);
//				break;
//			}
//			volnet = value;
//			topack = volnet / 1.15;//- Unit(volnet, 15, 'DIGIT', 'PERCENT');
//			volgross = volnet + Unit(volnet, 20, 'DIGIT', 'PERCENT');
//			volgross = volnet + (IsSameGross ? Unit(volnet, 20, 'DIGIT', 'PERCENT') : 0);//+ Unit(volnet, Percent, 'DIGIT', 'PERCENT');
//			wtnet = volnet * dens_fact;
//			wtgross = volgross * dens_fact;
//			acwt = ship_type == 'Air' ? wtgross : volgross * const_fact;
//			break;
//		case "VOL_GROSS":
//			if ((dens_fact <= 0 || !LOOSECASED || !LCLFCL) && actcontrol= 0) {
//				errmsg = "Dencity factor, Loose/Cased and LCL/FCL are required to calculate weight/volume.";
//				alert(errmsg);
//				break;
//			}
//			volgross = value;
//			volnet = IsSameGross ? (volgross / 1.2) : volgross;//- Unit(volgross, 20, 'DIGIT', 'PERCENT');
//			topack = volnet / 1.15;//- Unit(volnet, 15, 'DIGIT', 'PERCENT');
//			wtnet = volnet * dens_fact;
//			wtgross = volgross * dens_fact;
//			acwt = ship_type == 'Air' ? wtgross : volgross * const_fact;
//			break;
//		case "WT_NET":
//			if ((dens_fact <= 0 || !LOOSECASED || !LCLFCL) && actcontrol= 0) {
//				errmsg = "Dencity factor, Loose/Cased and LCL/FCL are required to calculate weight/volume.";
//				alert(errmsg);
//				break;
//			}
//			wtnet = value;
//			volnet = wtnet / dens_fact;
//			topack = volnet / 1.15;//- Unit(volnet, 15, 'DIGIT', 'PERCENT');
//			volgross = volnet + Unit(volnet, 20, 'DIGIT', 'PERCENT');
//			volgross = volnet + (IsSameGross ? Unit(volnet, 20, 'DIGIT', 'PERCENT') : 0); //Unit(volnet, Percent, 'DIGIT', 'PERCENT');
//			wtnet = volnet * dens_fact;
//			wtgross = volgross * dens_fact;
//			acwt = ship_type == 'Air' ? wtgross : volgross * const_fact;
//			break;
//		case "WT_GROSS":
//			if ((dens_fact <= 0 || !LOOSECASED || !LCLFCL) && actcontrol= 0) {
//				errmsg = "Dencity factor, Loose/Cased and LCL/FCL are required to calculate weight/volume.";
//				alert(errmsg);
//				break;
//			}
//			wtgross = value;
//			volgross = wtgross / dens_fact;
//			volnet = IsSameGross ? (volgross / 1.2) : volgross;//- Unit(volgross, 20, 'DIGIT', 'PERCENT');
//			topack = volnet / 1.15// - Unit(volnet, 15, 'DIGIT', 'PERCENT');
//			wtnet = volnet * dens_fact;
//			acwt = ship_type == 'Air' ? wtgross : volgross * const_fact;
//			break;
//		case "WT_ACWT":
//			if ((dens_fact <= 0 || !LOOSECASED || !LCLFCL) && actcontrol= 0) {
//				errmsg = "Dencity factor, Loose/Cased and LCL/FCL are required to calculate weight/volume.";
//				alert(errmsg);
//				break;
//			}
//			acwt = value;
//			if (ship_type == 'Air') {
//				wtgross = acwt;
//				volgross = wtgross / dens_fact;
//			}
//			else {
//				volgross = acwt / const_fact;
//				wtgross = volgross * dens_fact;
//			}
//			volnet = IsSameGross ? (volgross / 1.2) : volgross;//volgross / 1.2;//- Unit(volgross, 20, 'DIGIT', 'PERCENT');
//			topack = volnet / 1.15//- Unit(volnet, 15, 'DIGIT', 'PERCENT');
//			wtnet = volnet * dens_fact;
//			break;
//		default:
//	}

//    return result = { topack, volnet, volgross, wtnet, wtgross, acwt, errmsg };
//}

