using Newtonsoft.Json;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;

namespace RELOCBS.Common
{
    public class EInvoiceHandlerDebitNote
    {
        public class TranDtls
        {
            public string TaxSch { get; set; }
            public string SupTyp { get; set; }
            public string RegRev { get; set; }
            public string IgstOnIntra { get; set; }
        }

        public class DocDtls
        {
            public string Typ { get; set; }
            public string No { get; set; }
            public string Dt { get; set; }
        }

        public class SellerDtls
        {
            public string Gstin { get; set; }
            public string LglNm { get; set; }
            public string Addr1 { get; set; }
            public string Loc { get; set; }
            public string Pin { get; set; }
            public string Stcd { get; set; }
        }

        public class BuyerDtls
        {
            public string Gstin { get; set; }
            public string LglNm { get; set; }
            public string Pos { get; set; }
            public string Addr1 { get; set; }
            public string Loc { get; set; }
            public string Pin { get; set; }
            public string Stcd { get; set; }
        }

        public class ItemList
        {
            public string SlNo { get; set; }
            public string PrdDesc { get; set; }
            public string IsServc { get; set; }
            public string HsnCd { get; set; }
            public int Qty { get; set; }
            public string Unit { get; set; }
            public double UnitPrice { get; set; }
            public double TotAmt { get; set; }
            public double AssAmt { get; set; }
            public double GstRt { get; set; }

            public double IgstAmt { get; set; }
            public double CgstAmt { get; set; }
            public double SgstAmt { get; set; }

            public double OthChrg { get; set; }
            public double TotItemVal { get; set; }
        }

        public class ValDtls
        {
            public double AssVal { get; set; }
            public double CgstVal { get; set; }
            public double SgstVal { get; set; }
            public double IgstVal { get; set; }
            public double OthChrg { get; set; }
            public double TotInvVal { get; set; }
            public double TotInvValFc { get; set; }
        }

        public class Transaction
        {
            public string Version { get; set; }
            public TranDtls TranDtls { get; set; }
            public DocDtls DocDtls { get; set; }
            public SellerDtls SellerDtls { get; set; }
            public BuyerDtls BuyerDtls { get; set; }
            public List<ItemList> ItemList { get; set; }
            public ValDtls ValDtls { get; set; }
        }

        public class MyArray
        {
            public Transaction transaction { get; set; }
        }

        public class Root
        {
            public List<MyArray> MyArray { get; set; }
        }

        public string convertToInvoiceJson(DataTable DT)
        {
            string outJSON = "";

            Root R = new Root();
            List<MyArray> LM = new List<MyArray>();

            if (DT != null && DT.Rows.Count > 0)
            {
                TranDtls TD = new TranDtls();
                DocDtls DD = new DocDtls();
                SellerDtls SD = new SellerDtls();
                BuyerDtls BD = new BuyerDtls();

                ValDtls VD = new ValDtls();
                Transaction T = new Transaction();

                TD.TaxSch = Convert.ToString(DT.Rows[0]["TaxSch"]);
                TD.SupTyp = Convert.ToString(DT.Rows[0]["SupTyp"]);
                TD.RegRev = Convert.ToString(DT.Rows[0]["RegRev"]);
                TD.IgstOnIntra = Convert.ToString(DT.Rows[0]["IgstOnIntra"]);

                DD.Typ = Convert.ToString(DT.Rows[0]["Typ"]);
                DD.No = Convert.ToString(DT.Rows[0]["No"]);
                DD.Dt = Convert.ToString(DT.Rows[0]["Dt"]);

                SD.Gstin = Convert.ToString(DT.Rows[0]["S_Gstin"]);
                SD.LglNm = Convert.ToString(DT.Rows[0]["S_LglNm"]);
                SD.Addr1 = Convert.ToString(DT.Rows[0]["S_Addr1"]);
                SD.Loc = Convert.ToString(DT.Rows[0]["S_Loc"]);
                SD.Pin = Convert.ToString(DT.Rows[0]["S_Pin"]);
                SD.Stcd = Convert.ToString(DT.Rows[0]["S_Stcd"]);

                BD.Gstin = Convert.ToString(DT.Rows[0]["B_Gstin"]);
                BD.LglNm = Convert.ToString(DT.Rows[0]["B_LglNm"]);
                BD.Addr1 = Convert.ToString(DT.Rows[0]["B_Addr1"]);
                BD.Loc = Convert.ToString(DT.Rows[0]["B_Loc"]);
                BD.Pos = Convert.ToString(DT.Rows[0]["B_Pos"]);
                BD.Pin = Convert.ToString(DT.Rows[0]["B_Pin"]);
                BD.Stcd = Convert.ToString(DT.Rows[0]["B_Stcd"]);

                List<ItemList> li = new List<ItemList>();//In case of multiple Items need to iterate those here
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    ItemList IL = new ItemList();
                    IL.SlNo = Convert.ToString(DT.Rows[i]["SlNo"]);
                    IL.PrdDesc = Convert.ToString(DT.Rows[i]["PrdDesc"]);
                    IL.IsServc = Convert.ToString(DT.Rows[i]["IsServc"]);
                    IL.HsnCd = Convert.ToString(DT.Rows[i]["HsnCd"]);
                    IL.Qty = Convert.ToInt32(DT.Rows[i]["Qty"]);
                    IL.Unit = Convert.ToString(DT.Rows[i]["Unit"]);
                    IL.UnitPrice = Convert.ToDouble(DT.Rows[i]["UnitPrice"]);
                    IL.TotAmt = Convert.ToDouble(DT.Rows[i]["TotAmt"]);
                    IL.AssAmt = Convert.ToDouble(DT.Rows[i]["AssAmt"]);
                    IL.GstRt = Convert.ToDouble(DT.Rows[i]["GstRt"]);

                    IL.IgstAmt = Convert.ToDouble(DT.Rows[i]["IgstAmt"]);
                    IL.CgstAmt = Convert.ToDouble(DT.Rows[i]["CgstAmt"]);
                    IL.SgstAmt = Convert.ToDouble(DT.Rows[i]["SgstAmt"]);
                    IL.OthChrg = Convert.ToDouble(DT.Rows[i]["OthChrg"]);
                    IL.TotItemVal = Convert.ToDouble(DT.Rows[i]["TotItemVal"]);

                    li.Add(IL);
                }

                VD.AssVal = Convert.ToInt32(DT.Rows[0]["AssVal"]);
                VD.CgstVal = Convert.ToDouble(DT.Rows[0]["CgstVal"]);
                VD.SgstVal = Convert.ToDouble(DT.Rows[0]["SgstVal"]);
                VD.IgstVal = Convert.ToDouble(DT.Rows[0]["IgstVal"]);
                VD.OthChrg = Convert.ToDouble(DT.Rows[0]["OthChrgVal"]);
                VD.TotInvVal = Convert.ToDouble(DT.Rows[0]["TotInvVal"]);
                VD.TotInvValFc = Convert.ToDouble(DT.Rows[0]["TotInvValFc"]);

                T.Version = Convert.ToString(DT.Rows[0]["Version"]);
                T.TranDtls = TD;
                T.DocDtls = DD;
                T.SellerDtls = SD;
                T.BuyerDtls = BD;
                T.ItemList = li;
                T.ValDtls = VD;

                MyArray MA = new MyArray();
                MA.transaction = T;

                LM.Add(MA);

                R.MyArray = LM;
                JsonSerializerSettings s = new JsonSerializerSettings();

                outJSON = JsonConvert.SerializeObject(R);
                outJSON = outJSON.Replace("{\"MyArray\":", "");
                outJSON = outJSON.Remove(outJSON.Length - 1, 1);
            }
            return outJSON;
        }

        public string callEInvoiceAPI(string ownerID, string GSTIN, string authToken, string product, DataTable DT, string InvoiceNo)
        {
            try
            {
                //InvoiceNo = "CF2021102700244";
                string newFileName = InvoiceNo + "_APIParam.txt";
                string FilePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["APIJsonDataFile"]);
                using (FileStream fs = File.Create(Path.Combine(FilePath, newFileName)))
                {
                    // Add some text to file    
                    string param = "owner_id : " + ownerID + ", gstin : " + GSTIN + ", x-cleartax-auth-token : " + authToken + ", x-cleartax-product : " + product;
                    Byte[] title = new UTF8Encoding(true).GetBytes(param);

                    fs.Write(title, 0, title.Length);
                    //byte[] author = new UTF8Encoding(true).GetBytes("Mahesh Chand");
                    //fs.Write(author, 0, author.Length);
                }
                string output = "";
                string url = System.Configuration.ConfigurationManager.AppSettings["APIURL"].ToString();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/json; charset=utf-8"; //set the content type to JSON
                request.Method = "PUT"; //make an HTTP POST
                request.Headers.Add("owner_id", ownerID);
                request.Headers.Add("gstin", GSTIN);
                request.Headers.Add("x-cleartax-auth-token", authToken);
                request.Headers.Add("x-cleartax-product", product);

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string res = convertToInvoiceJson(DT);
                    //string res = "[{\"transaction\":{\"Version\":\"1.1\",\"TranDtls\":{\"TaxSch\":\"GST\",\"SupTyp\":\"CDNUR\",\"RegRev\":\"N\",\"IgstOnIntra\":\"N\"},\"DocDtls\":{\"Typ\":\"CRN\",\"No\":\"CF2021102700244\",\"Dt\":\"11/11/2020\"},\"SellerDtls\":{\"Gstin\":\"27AABCW6386P4ZC\",\"LglNm\":\"Writer Business Services Private Limited\",\"Addr1\":\"Dr. B. Ambedkar Road, Mumbai 400 033, India\",\"Loc\":\"MUMBAI\",\"Pin\":\"400033\",\"Stcd\":\"27\"},\"BuyerDtls\":{\"Gstin\":\"URP\",\"LglNm\":\"ALLIED INTERNANTIONAL.\",\"Pos\":\"96\",\"Addr1\":\"P.O.BOX 988,\",\"Loc\":\"FORT WAYNE\",\"Pin\":\"999999\",\"Stcd\":\"96\"},\"ItemList\":[{\"SlNo\":\"1\",\"PrdDesc\":\"TO ORIGIN SERVICES\",\"IsServc\":\"Y\",\"HsnCd\":\"996531\",\"UnitPrice\":9698.22,\"TotAmt\":9698.22,\"AssAmt\":9698.22,\"GstRt\":0.0,\"IgstAmt\":0.0,\"CgstAmt\":0.0,\"SgstAmt\":0.0,\"OthChrg\":0.0,\"TotItemVal\":9698.22}],\"ValDtls\":{\"AssVal\":9698.0,\"CgstVal\":0.0,\"SgstVal\":0.0,\"IgstVal\":0.0,\"OthChrg\":0.0,\"TotInvVal\":9698.22,\"TotInvValFc\":129.0}}}]";
                    newFileName = InvoiceNo + "_APIJSON.txt";
                    FilePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["APIJsonDataFile"]);
                    using (FileStream fs = File.Create(Path.Combine(FilePath, newFileName)))
                    {
                        // Add some text to file    
                        Byte[] title = new UTF8Encoding(true).GetBytes(res);

                        fs.Write(title, 0, title.Length);
                    }

                    streamWriter.Write(res);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                // Get the response.
                WebResponse response = request.GetResponse();
                var streamReader = new StreamReader(response.GetResponseStream());
                var result = streamReader.ReadToEnd();
                output = Convert.ToString(result);

                return output;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public byte[] generatecode(string qrText, bool IsSavedCode = false, string Filename = null, string AppName = null)
        {
            try
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrText,
                QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20);
                ////qrText = "eyJhbGciOiJSUzI1NiIsImtpZCI6IkVEQzU3REUxMzU4QjMwMEJBOUY3OTM0MEE2Njk2ODMxRjNDODUwNDciLCJ0eXAiOiJKV1QiLCJ4NXQiOiI3Y1Y5NFRXTE1BdXA5NU5BcG1sb01mUElVRWMifQ.eyJkYXRhIjoie1wiU2VsbGVyR3N0aW5cIjpcIjI5QUFGQ0Q1ODYyUjAwMFwiLFwiQnV5ZXJHc3RpblwiOlwiVVJQXCIsXCJEb2NOb1wiOlwiRkMyMDIxMjEwMDAwMDk0XCIsXCJEb2NUeXBcIjpcIklOVlwiLFwiRG9jRHRcIjpcIjIxLzA5LzIwMjBcIixcIlRvdEludlZhbFwiOjEyNzAwLjY4LFwiSXRlbUNudFwiOjQsXCJNYWluSHNuQ29kZVwiOlwiOTk2Nzk5XCIsXCJJcm5cIjpcImJlMmRjODgyOTcxNzBhNmZiYThlMzIyNjYyNjBiNjYyYWI3MWU2NjQyNGQ0M";
                ////object billid_para = this.ReportParameters["SPVARBILLID"].Value;
                //byte[] imageData = new byte[] { };
                ////string qrText = GETIRNQRCode(billid_para.ToString());

                //if (!string.IsNullOrEmpty(qrText))
                //{
                //	//QRCodeGenerator qrGenerator = new QRCodeGenerator();
                //	//QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.H);
                //	//System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
                //	GeneratedBarcode qrCode = QRCodeWriter.CreateQrCode(qrText, 500, QRCodeWriter.QrErrorCorrectionLevel.Medium);
                //	//imgBarCode.Height = 350;
                //	//imgBarCode	.Width = 350;                                           

                //	using (Bitmap bitMap = qrCode.ToBitmap())
                //	{
                //		//Bitmap resized = new Bitmap(bitMap, new Size(200, 200));
                //		using (MemoryStream ms = new MemoryStream())
                //		{
                //			if (IsSavedCode)
                //			{
                //				string newFileName = Filename + ".jpg";
                //				string FilePath = Path.Combine(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["InvoiceFile"]),AppName);
                //				if (!Directory.Exists(FilePath))
                //				{
                //					Directory.CreateDirectory(FilePath);
                //				}
                //				bitMap.Save(Path.Combine(FilePath, newFileName), ImageFormat.Jpeg);
                //			}
                //			else
                //			{
                //				bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                //				Image imageqr = Image.FromStream(ms);
                //				imageData = turnImageToByteArray(imageqr);
                //			}
                //		}
                //	}
                //}

                return BitmapToBytes(qrCodeImage, IsSavedCode, Filename, AppName);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private byte[] turnImageToByteArray(Image img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, ImageFormat.Bmp);
            return ms.ToArray();
        }

        private static Byte[] BitmapToBytes(Bitmap img, bool IsSavedCode, string Filename, string AppName)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                if (IsSavedCode)
                {
                    string newFileName = Filename + ".jpg";
                    string FilePath = Path.Combine(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["InvoiceFile"]), AppName);
                    if (!Directory.Exists(FilePath))
                    {
                        Directory.CreateDirectory(FilePath);
                    }

                    img.Save(Path.Combine(FilePath, newFileName), ImageFormat.Jpeg);
                    return stream.ToArray();
                }
                else
                {
                    img.Save(stream, ImageFormat.Png);
                    return stream.ToArray();
                }
            }
        }
    }
}