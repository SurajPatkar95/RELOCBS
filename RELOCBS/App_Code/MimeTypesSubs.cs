using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.InteropServices;
using System.IO;

namespace RELOCBS.App_Code
{
    public class MimeTypesSubs
    {

        private static List<string> knownTypes;

        private static Dictionary<string, string> mimeTypes;

        private static List<string> knownMagicNumbers;
        private static Dictionary<string, string> dicknownMagicNumbers;

        //    [DllImport("urlmon.dll", CharSet = CharSet.Auto)]
        //    private static extern UInt32 FindMimeFromData(
        //        UInt32 pBC, [MarshalAs(UnmanagedType.LPStr)]
        //    string pwzUrl, [MarshalAs(UnmanagedType.LPArray)]
        //    byte[] pBuffer, UInt32 cbSize, [MarshalAs(UnmanagedType.LPStr)]
        //    string pwzMimeProposed, UInt32 dwMimeFlags, ref UInt32 ppwzMimeOut, UInt32 dwReserverd
        //);

        [DllImport("urlmon.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = false)]
        static extern int FindMimeFromData(IntPtr pBC,
              [MarshalAs(UnmanagedType.LPWStr)] string pwzUrl,
             [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1, SizeParamIndex = 3)] 
        byte[] pBuffer,
              int cbSize,
                 [MarshalAs(UnmanagedType.LPWStr)]  string pwzMimeProposed,
              int dwMimeFlags,
              out IntPtr ppwzMimeOut,
              int dwReserved);



        public static string GetContentType(string fileName)
        {
            if (knownTypes == null || mimeTypes == null)
                InitializeMimeTypeLists();
            string contentType = "";
            string extension = System.IO.Path.GetExtension(fileName).Replace(".", "").ToLower();
            mimeTypes.TryGetValue(extension, out contentType);
            if (string.IsNullOrEmpty(contentType) || knownTypes.Contains(contentType))
            {
                string headerType = ScanFileForMimeType(fileName);
                if (headerType != "application/octet-stream" || string.IsNullOrEmpty(contentType))
                    contentType = headerType;
            }
            return contentType;
        }


        public static bool GetValidMimeType(HttpPostedFileBase fileName)
        {
            if (knownTypes == null || mimeTypes == null)
                InitializeMimeTypeLists();
            string contentType = "";
            string extension = Path.GetExtension(fileName.FileName).Replace(".", "").ToLower();
            mimeTypes.TryGetValue(extension, out contentType);
            if (string.IsNullOrEmpty(contentType) || knownTypes.Contains(contentType))
            {
                string headerType = ScanFileForMimeType(fileName);
                if (headerType == "application/octet-stream" || string.IsNullOrEmpty(contentType))
                    return true;
            }
            return false;
        }

        private static string ScanFileForMimeType(string fileName)
        {
            try
            {
                IntPtr mimeout;
                if (!System.IO.File.Exists(fileName))
                    throw new FileNotFoundException(fileName + " not found");

                int MaxContent = (int)new FileInfo(fileName).Length;
                if (MaxContent > 4096) MaxContent = 4096;
                FileStream fs = File.OpenRead(fileName);


                byte[] buf = new byte[MaxContent];
                fs.Read(buf, 0, MaxContent);
                fs.Close();
                int result = FindMimeFromData(IntPtr.Zero, fileName, buf, MaxContent, null, 0, out mimeout, 0);

                if (result != 0)
                    throw Marshal.GetExceptionForHR(result);
                string mime = Marshal.PtrToStringUni(mimeout);
                Marshal.FreeCoTaskMem(mimeout);
                return mime;
            }
            catch (Exception ex)
            {
                return "application/octet-stream";
            }
        }

        private static string ScanFileForMimeType(HttpPostedFileBase fileName)
        {
            try
            {
                byte[] buffer = new byte[4096];

                using (Stream fs = fileName.InputStream)
                {
                    int readLength = Convert.ToInt32(Math.Min(4096, fs.Length));
                     fs.Read(buffer, 0, readLength);
                }

                //using (FileStream fs = new FileStream(fileName, FileMode.Open))
                //{
                //    int readLength = Convert.ToInt32(Math.Min(256, fs.Length));
                //    fs.Read(buffer, 0, readLength);
                //}

                IntPtr mimeTypePtr;//= new IntPtr(mimeType);
                int result = FindMimeFromData(IntPtr.Zero, null, buffer, 4096, null, 0, out mimeTypePtr, 0);
                string mime = Marshal.PtrToStringUni(mimeTypePtr);
                Marshal.FreeCoTaskMem(mimeTypePtr);
                if (string.IsNullOrEmpty(mime))
                    mime = "application/octet-stream";
                return mime;
            }
            catch (Exception ex)
            {
                return "application/octet-stream";
            }
        }

        private static void InitializeMimeTypeLists()
        {
            knownTypes = new string[] {
        "text/plain",
        "text/html",
        "text/xml",
        "text/richtext",
        "text/scriptlet",
        "audio/x-aiff",
        "audio/basic",
        "audio/mid",
        "audio/wav",
        "image/gif",
        "image/jpeg",
        "image/pjpeg",
        "image/png",
        "image/x-png",
        "image/tiff",
        "image/bmp",
        "image/x-xbitmap",
        "image/x-jg",
        "image/x-emf",
        "image/x-wmf",
        "video/avi",
        "video/mpeg",
        "video/x-flv",
        "video/mp4",
        "application/x-mpegURL",
        "video/MP2T",
        "video/3gpp",
        "video/quicktime",
        "video/x-msvideo",
        "video/x-ms-wmv",
        "application/octet-stream",
        "application/postscript",
        "application/base64",
        "application/macbinhex40",
        "application/pdf",
        "application/xml",
        "application/atom+xml",
        "application/rss+xml",
        "application/x-compressed",
        "application/x-zip-compressed",
        "application/x-gzip-compressed",
        "application/java",
        "application/x-msdownload",
        
    }.ToList();

            mimeTypes = new Dictionary<string, string>();
            mimeTypes.Add("3dm", "x-world/x-3dmf");
            mimeTypes.Add("3dmf", "x-world/x-3dmf");
            mimeTypes.Add("a", "application/octet-stream");
            mimeTypes.Add("aab", "application/x-authorware-bin");
            mimeTypes.Add("aam", "application/x-authorware-map");
            mimeTypes.Add("aas", "application/x-authorware-seg");
            mimeTypes.Add("abc", "text/vnd.abc");
            mimeTypes.Add("acgi", "text/html");
            mimeTypes.Add("afl", "video/animaflex");
            mimeTypes.Add("ai", "application/postscript");
            mimeTypes.Add("aif", "audio/aiff");
            mimeTypes.Add("aifc", "audio/aiff");
            mimeTypes.Add("aiff", "audio/aiff");
            mimeTypes.Add("aim", "application/x-aim");
            mimeTypes.Add("aip", "text/x-audiosoft-intra");
            mimeTypes.Add("ani", "application/x-navi-animation");
            mimeTypes.Add("aos", "application/x-nokia-9000-communicator-add-on-software");
            mimeTypes.Add("aps", "application/mime");
            mimeTypes.Add("arc", "application/octet-stream");
            mimeTypes.Add("arj", "application/arj");
            mimeTypes.Add("art", "image/x-jg");
            mimeTypes.Add("asf", "video/x-ms-asf");
            mimeTypes.Add("asm", "text/x-asm");
            mimeTypes.Add("asp", "text/asp");
            mimeTypes.Add("asx", "application/x-mplayer2");
            mimeTypes.Add("au", "audio/basic");
            mimeTypes.Add("avi", "video/avi");
            mimeTypes.Add("avs", "video/avs-video");
            mimeTypes.Add("bcpio", "application/x-bcpio");
            mimeTypes.Add("bin", "application/octet-stream");
            mimeTypes.Add("bm", "image/bmp");
            mimeTypes.Add("bmp", "image/bmp");
            mimeTypes.Add("boo", "application/book");
            mimeTypes.Add("book", "application/book");
            mimeTypes.Add("boz", "application/x-bzip2");
            mimeTypes.Add("bsh", "application/x-bsh");
            mimeTypes.Add("bz", "application/x-bzip");
            mimeTypes.Add("bz2", "application/x-bzip2");
            mimeTypes.Add("c", "text/plain");
            mimeTypes.Add("c++", "text/plain");
            mimeTypes.Add("cat", "application/vnd.ms-pki.seccat");
            mimeTypes.Add("cc", "text/plain");
            mimeTypes.Add("ccad", "application/clariscad");
            mimeTypes.Add("cco", "application/x-cocoa");
            mimeTypes.Add("cdf", "application/cdf");
            mimeTypes.Add("cer", "application/pkix-cert");
            mimeTypes.Add("cha", "application/x-chat");
            mimeTypes.Add("chat", "application/x-chat");
            mimeTypes.Add("class", "application/java");
            mimeTypes.Add("com", "application/octet-stream");
            mimeTypes.Add("conf", "text/plain");
            mimeTypes.Add("cpio", "application/x-cpio");
            mimeTypes.Add("cpp", "text/x-c");
            mimeTypes.Add("cpt", "application/x-cpt");
            mimeTypes.Add("crl", "application/pkcs-crl");
            mimeTypes.Add("css", "text/css");
            mimeTypes.Add("def", "text/plain");
            mimeTypes.Add("der", "application/x-x509-ca-cert");
            mimeTypes.Add("dif", "video/x-dv");
            mimeTypes.Add("dir", "application/x-director");
            mimeTypes.Add("dl", "video/dl");
            mimeTypes.Add("doc", "application/msword");
            mimeTypes.Add("dot", "application/msword");
            mimeTypes.Add("dp", "application/commonground");
            mimeTypes.Add("drw", "application/drafting");
            mimeTypes.Add("dump", "application/octet-stream");
            mimeTypes.Add("dv", "video/x-dv");
            mimeTypes.Add("dvi", "application/x-dvi");
            mimeTypes.Add("dwf", "drawing/x-dwf (old)");
            mimeTypes.Add("dwg", "application/acad");
            mimeTypes.Add("dxf", "application/dxf");
            mimeTypes.Add("eps", "application/postscript");
            mimeTypes.Add("es", "application/x-esrehber");
            mimeTypes.Add("etx", "text/x-setext");
            mimeTypes.Add("evy", "application/envoy");
            mimeTypes.Add("exe", "application/octet-stream");
            mimeTypes.Add("f", "text/plain");
            mimeTypes.Add("f90", "text/x-fortran");
            mimeTypes.Add("fdf", "application/vnd.fdf");
            mimeTypes.Add("fif", "image/fif");
            mimeTypes.Add("fli", "video/fli");
            mimeTypes.Add("flv", "video/x-flv");
            mimeTypes.Add("for", "text/x-fortran");
            mimeTypes.Add("fpx", "image/vnd.fpx");
            mimeTypes.Add("g", "text/plain");
            mimeTypes.Add("g3", "image/g3fax");
            mimeTypes.Add("gif", "image/gif");
            mimeTypes.Add("gl", "video/gl");
            mimeTypes.Add("gsd", "audio/x-gsm");
            mimeTypes.Add("gtar", "application/x-gtar");
            mimeTypes.Add("gz", "application/x-compressed");
            mimeTypes.Add("h", "text/plain");
            mimeTypes.Add("help", "application/x-helpfile");
            mimeTypes.Add("hgl", "application/vnd.hp-hpgl");
            mimeTypes.Add("hh", "text/plain");
            mimeTypes.Add("hlp", "application/x-winhelp");
            mimeTypes.Add("htc", "text/x-component");
            mimeTypes.Add("htm", "text/html");
            mimeTypes.Add("html", "text/html");
            mimeTypes.Add("htmls", "text/html");
            mimeTypes.Add("htt", "text/webviewhtml");
            mimeTypes.Add("htx", "text/html");
            mimeTypes.Add("ice", "x-conference/x-cooltalk");
            mimeTypes.Add("ico", "image/x-icon");
            mimeTypes.Add("idc", "text/plain");
            mimeTypes.Add("ief", "image/ief");
            mimeTypes.Add("iefs", "image/ief");
            mimeTypes.Add("iges", "application/iges");
            mimeTypes.Add("igs", "application/iges");
            mimeTypes.Add("ima", "application/x-ima");
            mimeTypes.Add("imap", "application/x-httpd-imap");
            mimeTypes.Add("inf", "application/inf");
            mimeTypes.Add("ins", "application/x-internett-signup");
            mimeTypes.Add("ip", "application/x-ip2");
            mimeTypes.Add("isu", "video/x-isvideo");
            mimeTypes.Add("it", "audio/it");
            mimeTypes.Add("iv", "application/x-inventor");
            mimeTypes.Add("ivr", "i-world/i-vrml");
            mimeTypes.Add("ivy", "application/x-livescreen");
            mimeTypes.Add("jam", "audio/x-jam");
            mimeTypes.Add("jav", "text/plain");
            mimeTypes.Add("java", "text/plain");
            mimeTypes.Add("jcm", "application/x-java-commerce");
            mimeTypes.Add("jfif", "image/jpeg");
            mimeTypes.Add("jfif-tbnl", "image/jpeg");
            mimeTypes.Add("jpe", "image/jpeg");
            mimeTypes.Add("jpeg", "image/jpeg");
            mimeTypes.Add("jpg", "image/jpeg");
            mimeTypes.Add("jps", "image/x-jps");
            mimeTypes.Add("js", "application/x-javascript");
            mimeTypes.Add("jut", "image/jutvision");
            mimeTypes.Add("kar", "audio/midi");
            mimeTypes.Add("ksh", "application/x-ksh");
            mimeTypes.Add("la", "audio/nspaudio");
            mimeTypes.Add("lam", "audio/x-liveaudio");
            mimeTypes.Add("latex", "application/x-latex");
            mimeTypes.Add("lha", "application/lha");
            mimeTypes.Add("lhx", "application/octet-stream");
            mimeTypes.Add("list", "text/plain");
            mimeTypes.Add("lma", "audio/nspaudio");
            mimeTypes.Add("log", "text/plain");
            mimeTypes.Add("lsp", "application/x-lisp");
            mimeTypes.Add("lst", "text/plain");
            mimeTypes.Add("lsx", "text/x-la-asf");
            mimeTypes.Add("ltx", "application/x-latex");
            mimeTypes.Add("lzh", "application/octet-stream");
            mimeTypes.Add("lzx", "application/lzx");
            mimeTypes.Add("m", "text/plain");
            mimeTypes.Add("m1v", "video/mpeg");
            mimeTypes.Add("m2a", "audio/mpeg");
            mimeTypes.Add("m2v", "video/mpeg");
            mimeTypes.Add("m3u", "audio/x-mpequrl");
            mimeTypes.Add("man", "application/x-troff-man");
            mimeTypes.Add("map", "application/x-navimap");
            mimeTypes.Add("mar", "text/plain");
            mimeTypes.Add("mbd", "application/mbedlet");
            mimeTypes.Add("mc$", "application/x-magic-cap-package-1.0");
            mimeTypes.Add("mcd", "application/mcad");
            mimeTypes.Add("mcf", "image/vasa");
            mimeTypes.Add("mcp", "application/netmc");
            mimeTypes.Add("me", "application/x-troff-me");
            mimeTypes.Add("mht", "message/rfc822");
            mimeTypes.Add("mhtml", "message/rfc822");
            mimeTypes.Add("mid", "audio/midi");
            mimeTypes.Add("midi", "audio/midi");
            mimeTypes.Add("mif", "application/x-frame");
            mimeTypes.Add("mime", "message/rfc822");
            mimeTypes.Add("mjf", "audio/x-vnd.audioexplosion.mjuicemediafile");
            mimeTypes.Add("mjpg", "video/x-motion-jpeg");
            mimeTypes.Add("mm", "application/base64");
            mimeTypes.Add("mme", "application/base64");
            mimeTypes.Add("mod", "audio/mod");
            mimeTypes.Add("moov", "video/quicktime");
            mimeTypes.Add("mov", "video/quicktime");
            mimeTypes.Add("movie", "video/x-sgi-movie");
            mimeTypes.Add("mp2", "audio/mpeg");
            mimeTypes.Add("mp3", "audio/mpeg3");
            mimeTypes.Add("mpa", "audio/mpeg");
            mimeTypes.Add("mpc", "application/x-project");
            mimeTypes.Add("mpe", "video/mpeg");
            mimeTypes.Add("mpeg", "video/mpeg");
            mimeTypes.Add("mpg", "video/mpeg");
            mimeTypes.Add("mpga", "audio/mpeg");
            mimeTypes.Add("mpp", "application/vnd.ms-project");
            mimeTypes.Add("mpt", "application/x-project");
            mimeTypes.Add("mpv", "application/x-project");
            mimeTypes.Add("mpx", "application/x-project");
            mimeTypes.Add("mrc", "application/marc");
            mimeTypes.Add("ms", "application/x-troff-ms");
            mimeTypes.Add("mv", "video/x-sgi-movie");
            mimeTypes.Add("my", "audio/make");
            mimeTypes.Add("mzz", "application/x-vnd.audioexplosion.mzz");
            mimeTypes.Add("nap", "image/naplps");
            mimeTypes.Add("naplps", "image/naplps");
            mimeTypes.Add("nc", "application/x-netcdf");
            mimeTypes.Add("ncm", "application/vnd.nokia.configuration-message");
            mimeTypes.Add("nif", "image/x-niff");
            mimeTypes.Add("niff", "image/x-niff");
            mimeTypes.Add("nix", "application/x-mix-transfer");
            mimeTypes.Add("nsc", "application/x-conference");
            mimeTypes.Add("nvd", "application/x-navidoc");
            mimeTypes.Add("o", "application/octet-stream");
            mimeTypes.Add("oda", "application/oda");
            mimeTypes.Add("omc", "application/x-omc");
            mimeTypes.Add("omcd", "application/x-omcdatamaker");
            mimeTypes.Add("omcr", "application/x-omcregerator");
            mimeTypes.Add("p", "text/x-pascal");
            mimeTypes.Add("p10", "application/pkcs10");
            mimeTypes.Add("p12", "application/pkcs-12");
            mimeTypes.Add("p7a", "application/x-pkcs7-signature");
            mimeTypes.Add("p7c", "application/pkcs7-mime");
            mimeTypes.Add("pas", "text/pascal");
            mimeTypes.Add("pbm", "image/x-portable-bitmap");
            mimeTypes.Add("pcl", "application/vnd.hp-pcl");
            mimeTypes.Add("pct", "image/x-pict");
            mimeTypes.Add("pcx", "image/x-pcx");
            mimeTypes.Add("pdf", "application/pdf");
            mimeTypes.Add("pfunk", "audio/make");
            mimeTypes.Add("pgm", "image/x-portable-graymap");
            mimeTypes.Add("pic", "image/pict");
            mimeTypes.Add("pict", "image/pict");
            mimeTypes.Add("pkg", "application/x-newton-compatible-pkg");
            mimeTypes.Add("pko", "application/vnd.ms-pki.pko");
            mimeTypes.Add("pl", "text/plain");
            mimeTypes.Add("plx", "application/x-pixclscript");
            mimeTypes.Add("pm", "image/x-xpixmap");
            mimeTypes.Add("png", "image/png");
            mimeTypes.Add("pnm", "application/x-portable-anymap");
            mimeTypes.Add("pot", "application/mspowerpoint");
            mimeTypes.Add("pov", "model/x-pov");
            mimeTypes.Add("ppa", "application/vnd.ms-powerpoint");
            mimeTypes.Add("ppm", "image/x-portable-pixmap");
            mimeTypes.Add("pps", "application/mspowerpoint");
            mimeTypes.Add("ppt", "application/mspowerpoint");
            mimeTypes.Add("ppz", "application/mspowerpoint");
            mimeTypes.Add("pre", "application/x-freelance");
            mimeTypes.Add("prt", "application/pro_eng");
            mimeTypes.Add("ps", "application/postscript");
            mimeTypes.Add("psd", "application/octet-stream");
            mimeTypes.Add("pvu", "paleovu/x-pv");
            mimeTypes.Add("pwz", "application/vnd.ms-powerpoint");
            mimeTypes.Add("py", "text/x-script.phyton");
            mimeTypes.Add("pyc", "applicaiton/x-bytecode.python");
            mimeTypes.Add("qcp", "audio/vnd.qcelp");
            mimeTypes.Add("qd3", "x-world/x-3dmf");
            mimeTypes.Add("qd3d", "x-world/x-3dmf");
            mimeTypes.Add("qif", "image/x-quicktime");
            mimeTypes.Add("qt", "video/quicktime");
            mimeTypes.Add("qtc", "video/x-qtc");
            mimeTypes.Add("qti", "image/x-quicktime");
            mimeTypes.Add("qtif", "image/x-quicktime");
            mimeTypes.Add("ra", "audio/x-pn-realaudio");
            mimeTypes.Add("ram", "audio/x-pn-realaudio");
            mimeTypes.Add("ras", "application/x-cmu-raster");
            mimeTypes.Add("rast", "image/cmu-raster");
            mimeTypes.Add("rexx", "text/x-script.rexx");
            mimeTypes.Add("rf", "image/vnd.rn-realflash");
            mimeTypes.Add("rgb", "image/x-rgb");
            mimeTypes.Add("rm", "application/vnd.rn-realmedia");
            mimeTypes.Add("rmi", "audio/mid");
            mimeTypes.Add("rmm", "audio/x-pn-realaudio");
            mimeTypes.Add("rmp", "audio/x-pn-realaudio");
            mimeTypes.Add("rng", "application/ringing-tones");
            mimeTypes.Add("rnx", "application/vnd.rn-realplayer");
            mimeTypes.Add("roff", "application/x-troff");
            mimeTypes.Add("rp", "image/vnd.rn-realpix");
            mimeTypes.Add("rpm", "audio/x-pn-realaudio-plugin");
            mimeTypes.Add("rt", "text/richtext");
            mimeTypes.Add("rtf", "text/richtext");
            mimeTypes.Add("rtx", "application/rtf");
            mimeTypes.Add("rv", "video/vnd.rn-realvideo");
            mimeTypes.Add("s", "text/x-asm");
            mimeTypes.Add("s3m", "audio/s3m");
            mimeTypes.Add("saveme", "application/octet-stream");
            mimeTypes.Add("sbk", "application/x-tbook");
            mimeTypes.Add("scm", "application/x-lotusscreencam");
            mimeTypes.Add("sdml", "text/plain");
            mimeTypes.Add("sdp", "application/sdp");
            mimeTypes.Add("sdr", "application/sounder");
            mimeTypes.Add("sea", "application/sea");
            mimeTypes.Add("set", "application/set");
            mimeTypes.Add("sgm", "text/sgml");
            mimeTypes.Add("sgml", "text/sgml");
            mimeTypes.Add("sh", "application/x-bsh");
            mimeTypes.Add("shtml", "text/html");
            mimeTypes.Add("sid", "audio/x-psid");
            mimeTypes.Add("sit", "application/x-sit");
            mimeTypes.Add("skd", "application/x-koan");
            mimeTypes.Add("skm", "application/x-koan");
            mimeTypes.Add("skp", "application/x-koan");
            mimeTypes.Add("skt", "application/x-koan");
            mimeTypes.Add("sl", "application/x-seelogo");
            mimeTypes.Add("smi", "application/smil");
            mimeTypes.Add("smil", "application/smil");
            mimeTypes.Add("snd", "audio/basic");
            mimeTypes.Add("sol", "application/solids");
            mimeTypes.Add("spc", "application/x-pkcs7-certificates");
            mimeTypes.Add("spl", "application/futuresplash");
            mimeTypes.Add("spr", "application/x-sprite");
            mimeTypes.Add("sprite", "application/x-sprite");
            mimeTypes.Add("src", "application/x-wais-source");
            mimeTypes.Add("ssi", "text/x-server-parsed-html");
            mimeTypes.Add("ssm", "application/streamingmedia");
            mimeTypes.Add("sst", "application/vnd.ms-pki.certstore");
            mimeTypes.Add("step", "application/step");
            mimeTypes.Add("stl", "application/sla");
            mimeTypes.Add("stp", "application/step");
            mimeTypes.Add("sv4cpio", "application/x-sv4cpio");
            mimeTypes.Add("sv4crc", "application/x-sv4crc");
            mimeTypes.Add("svf", "image/vnd.dwg");
            mimeTypes.Add("svr", "application/x-world");
            mimeTypes.Add("swf", "application/x-shockwave-flash");
            mimeTypes.Add("t", "application/x-troff");
            mimeTypes.Add("talk", "text/x-speech");
            mimeTypes.Add("tar", "application/x-tar");
            mimeTypes.Add("tbk", "application/toolbook");
            mimeTypes.Add("tcl", "application/x-tcl");
            mimeTypes.Add("tcsh", "text/x-script.tcsh");
            mimeTypes.Add("tex", "application/x-tex");
            mimeTypes.Add("texi", "application/x-texinfo");
            mimeTypes.Add("texinfo", "application/x-texinfo");
            mimeTypes.Add("text", "text/plain");
            mimeTypes.Add("tgz", "application/x-compressed");
            mimeTypes.Add("tif", "image/tiff");
            mimeTypes.Add("tr", "application/x-troff");
            mimeTypes.Add("tsi", "audio/tsp-audio");
            mimeTypes.Add("tsp", "audio/tsplayer");
            mimeTypes.Add("tsv", "text/tab-separated-values");
            mimeTypes.Add("turbot", "image/florian");
            mimeTypes.Add("txt", "text/plain");
            mimeTypes.Add("uil", "text/x-uil");
            mimeTypes.Add("uni", "text/uri-list");
            mimeTypes.Add("unis", "text/uri-list");
            mimeTypes.Add("unv", "application/i-deas");
            mimeTypes.Add("uri", "text/uri-list");
            mimeTypes.Add("uris", "text/uri-list");
            mimeTypes.Add("ustar", "application/x-ustar");
            mimeTypes.Add("uu", "application/octet-stream");
            mimeTypes.Add("vcd", "application/x-cdlink");
            mimeTypes.Add("vcs", "text/x-vcalendar");
            mimeTypes.Add("vda", "application/vda");
            mimeTypes.Add("vdo", "video/vdo");
            mimeTypes.Add("vew", "application/groupwise");
            mimeTypes.Add("viv", "video/vivo");
            mimeTypes.Add("vivo", "video/vivo");
            mimeTypes.Add("vmd", "application/vocaltec-media-desc");
            mimeTypes.Add("vmf", "application/vocaltec-media-file");
            mimeTypes.Add("voc", "audio/voc");
            mimeTypes.Add("vos", "video/vosaic");
            mimeTypes.Add("vox", "audio/voxware");
            mimeTypes.Add("vqe", "audio/x-twinvq-plugin");
            mimeTypes.Add("vqf", "audio/x-twinvq");
            mimeTypes.Add("vql", "audio/x-twinvq-plugin");
            mimeTypes.Add("vrml", "application/x-vrml");
            mimeTypes.Add("vrt", "x-world/x-vrt");
            mimeTypes.Add("vsd", "application/x-visio");
            mimeTypes.Add("vst", "application/x-visio");
            mimeTypes.Add("vsw", "application/x-visio");
            mimeTypes.Add("w60", "application/wordperfect6.0");
            mimeTypes.Add("w61", "application/wordperfect6.1");
            mimeTypes.Add("w6w", "application/msword");
            mimeTypes.Add("wav", "audio/wav");
            mimeTypes.Add("wb1", "application/x-qpro");
            mimeTypes.Add("wbmp", "image/vnd.wap.wbmp");
            mimeTypes.Add("web", "application/vnd.xara");
            mimeTypes.Add("wiz", "application/msword");
            mimeTypes.Add("wk1", "application/x-123");
            mimeTypes.Add("wmf", "windows/metafile");
            mimeTypes.Add("wml", "text/vnd.wap.wml");
            mimeTypes.Add("wmlc", "application/vnd.wap.wmlc");
            mimeTypes.Add("wmls", "text/vnd.wap.wmlscript");
            mimeTypes.Add("wmlsc", "application/vnd.wap.wmlscriptc");
            mimeTypes.Add("word", "application/msword");
            mimeTypes.Add("wp", "application/wordperfect");
            mimeTypes.Add("wp5", "application/wordperfect");
            mimeTypes.Add("wp6", "application/wordperfect");
            mimeTypes.Add("wpd", "application/wordperfect");
            mimeTypes.Add("wq1", "application/x-lotus");
            mimeTypes.Add("wri", "application/mswrite");
            mimeTypes.Add("wrl", "application/x-world");
            mimeTypes.Add("wrz", "model/vrml");
            mimeTypes.Add("wsc", "text/scriplet");
            mimeTypes.Add("wsrc", "application/x-wais-source");
            mimeTypes.Add("wtk", "application/x-wintalk");
            mimeTypes.Add("xbm", "image/x-xbitmap");
            mimeTypes.Add("xdr", "video/x-amt-demorun");
            mimeTypes.Add("xgz", "xgl/drawing");
            mimeTypes.Add("xif", "image/vnd.xiff");
            mimeTypes.Add("xl", "application/excel");
            mimeTypes.Add("xla", "application/excel");
            mimeTypes.Add("xlb", "application/excel");
            mimeTypes.Add("xlc", "application/excel");
            mimeTypes.Add("xld", "application/excel");
            mimeTypes.Add("xlk", "application/excel");
            mimeTypes.Add("xll", "application/excel");
            mimeTypes.Add("xlm", "application/excel");
            mimeTypes.Add("xls", "application/excel");
            mimeTypes.Add("xlsx", "application/excel");
            mimeTypes.Add("xlt", "application/excel");
            mimeTypes.Add("xlv", "application/excel");
            mimeTypes.Add("xlw", "application/excel");
            mimeTypes.Add("xm", "audio/xm");
            mimeTypes.Add("xml", "text/xml");
            mimeTypes.Add("xmz", "xgl/movie");
            mimeTypes.Add("xpix", "application/x-vnd.ls-xpix");
            mimeTypes.Add("xpm", "image/x-xpixmap");
            mimeTypes.Add("x-png", "image/png");
            mimeTypes.Add("xsr", "video/x-amt-showrun");
            mimeTypes.Add("xwd", "image/x-xwd");
            mimeTypes.Add("xyz", "chemical/x-pdb");
            mimeTypes.Add("z", "application/x-compress");
            mimeTypes.Add("zip", "application/x-compressed");
            mimeTypes.Add("zoo", "application/octet-stream");
            mimeTypes.Add("zsh", "text/x-script.zsh");
            mimeTypes.Add("csv", "application/vnd.ms-excel");
            mimeTypes.Add("docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            mimeTypes.Add("dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template");
            mimeTypes.Add("docm","application/vnd.ms-word.document.macroEnabled.12");
            mimeTypes.Add("dotm", "application/vnd.ms-word.template.macroEnabled.12");
            mimeTypes.Add("pptx","application/vnd.openxmlformats-officedocument.presentationml.presentation");
            mimeTypes.Add("potx","application/vnd.openxmlformats-officedocument.presentationml.template");
            mimeTypes.Add("ppsx","application/vnd.openxmlformats-officedocument.presentationml.slideshow");
            mimeTypes.Add("ppam","application/vnd.ms-powerpoint.addin.macroEnabled.12");
            mimeTypes.Add("pptm","application/vnd.ms-powerpoint.presentation.macroEnabled.12");
            mimeTypes.Add("potm","application/vnd.ms-powerpoint.template.macroEnabled.12");
            mimeTypes.Add("ppsm", "application/vnd.ms-powerpoint.slideshow.macroEnabled.12");

            //mimeTypes.Add("flv", "video/x-flv");
            mimeTypes.Add("mp4", "video/mp4");
            mimeTypes.Add("m3u8", "application/x-mpegURL");
            mimeTypes.Add("ts", "video/MP2T");
            mimeTypes.Add("3gp", "video/3gpp");
           // mimeTypes.Add("mov", "video/quicktime");
            //mimeTypes.Add("avi", "video/x-msvideo");
            mimeTypes.Add("wmv", "video/x-ms-wmv");



            #region magic no
            dicknownMagicNumbers = new Dictionary<string, string>();
            dicknownMagicNumbers.Add("pdf", "25-50");
            dicknownMagicNumbers.Add("png", "89-50");
            dicknownMagicNumbers.Add("tif", "49-49");
            dicknownMagicNumbers.Add("gif", "47-49");
            dicknownMagicNumbers.Add("jpg", "FF-D8");
            dicknownMagicNumbers.Add("jpeg", "FF-D8");
            dicknownMagicNumbers.Add("bmp", "42-4D");
            dicknownMagicNumbers.Add("ico", "00-00");
            dicknownMagicNumbers.Add("mp4", "00-00");
            dicknownMagicNumbers.Add("vob", "00-00");
            dicknownMagicNumbers.Add("mp3", "49-44");
            dicknownMagicNumbers.Add("avi", "52-49");
            dicknownMagicNumbers.Add("wav", "52-49");
            dicknownMagicNumbers.Add("flv", "46-4C");
            dicknownMagicNumbers.Add("mov", "6D-6F");
            dicknownMagicNumbers.Add("wmv", "30-26");
            dicknownMagicNumbers.Add("wma", "30-26");
            dicknownMagicNumbers.Add("asf", "30-26");
            dicknownMagicNumbers.Add("zip", "50-4B");
            dicknownMagicNumbers.Add("docx", "50-4B");
            dicknownMagicNumbers.Add("xlsx", "50-4B");
            dicknownMagicNumbers.Add("pptx", "50-4B");
            dicknownMagicNumbers.Add("gz", "1F-8B");
            dicknownMagicNumbers.Add("tar", "75-73");
            dicknownMagicNumbers.Add("rar", "52-61");
            dicknownMagicNumbers.Add("pst", "21-42");
            dicknownMagicNumbers.Add("doc", "D0-CF");
            dicknownMagicNumbers.Add("sls", "D0-CF");
            dicknownMagicNumbers.Add("ppt", "D0-CF");
            dicknownMagicNumbers.Add("vsd", "D0-CF");
            dicknownMagicNumbers.Add("msg", "D0-CF");
            dicknownMagicNumbers.Add("xls", "50-4B");   //Collapse   D0-CF  -  50-4B 
            dicknownMagicNumbers.Add("rtf", "7B-5C");
            dicknownMagicNumbers.Add("3gp", "66-74");
            dicknownMagicNumbers.Add("dat", "50-4D");
            dicknownMagicNumbers.Add("xml", "3c-3f");
            dicknownMagicNumbers.Add("csv", "43-61");
            dicknownMagicNumbers.Add("db", "53-51");
            dicknownMagicNumbers.Add("swf", "43-57");
            dicknownMagicNumbers.Add("7z", "37-7A");
            knownMagicNumbers = new string[] {
                 "25-50",//PDF  -- 
                 "42-4D",//bmp --
                 "FF-D8",//jpg --
                 "47-49",//GIF --
                 "49-49",//tif --
                 "89-50",//png --
                 "00-00",//ico,mp4,vob
                 "49-44",//mp3
                 "52-49",//avi,wav
                 "46-4C",//flv
                 "6D-6F",//mov
                 "30-26",//wmv,wma,asf
                 "50-4B",//zip,docx,xlsx,pptx,
                 "1F-8B",//gz
                 "75-73",//tar
                 "52-61 ",//rar
                 "21-42",//pst
                 "D0-CF",//doc,sls,ppt,vsd,msg,xls
                 "7B-5C",//rtf
                 "66-74",//3gp
                 "50-4D",//dat
                 "3c-3f",//xml
                 "43-61", //CSV
                 "53-51", //db
                 "43-57", //swf
                 "46-57", //swf
                 "37-7A" //7z
                 }.ToList();
            #endregion



        }

        public static bool checkMagicNumbers(HttpPostedFileBase fileName)
        {
            string contentType = "";
            //string fileExt = fileName.GetExtension();
            string extension = Path.GetExtension(fileName.FileName).Replace(".", "").ToLower();
            mimeTypes.TryGetValue(extension, out contentType);
            if (contentType != "text/plain")
            {
                using (Stream checkStream = fileName.InputStream)
                {
                    BinaryReader chkBinary = new BinaryReader(checkStream);
                    Byte[] chkbytes = chkBinary.ReadBytes(0x10);

                    string data_as_hex = BitConverter.ToString(chkbytes);
                    string magicCheck = data_as_hex.Substring(0, 5);
                    if (extension == "xls" && magicCheck == "D0-CF")
                    {
                        magicCheck = magicCheck.Replace("D0-CF", "50-4B");
                    }
                    if (extension == "csv" && magicCheck == "56-65")
                    {
                        magicCheck = magicCheck.Replace("56-65", "43-61");
                    }
					//if (extension == "xlsx")
					//{
						//magicCheck = magicCheck.Replace("56-65", "43-61");
					//}
					if (string.IsNullOrEmpty(magicCheck) || !knownMagicNumbers.Contains(magicCheck) || dicknownMagicNumbers[extension] != magicCheck)
                    {
                        return true;//insert code here
                    }
                }
            }
            return false;
        }
    }
}