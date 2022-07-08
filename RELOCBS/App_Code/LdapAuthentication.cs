using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices;
using System.Data;
using System.Text;

namespace RELOCBS.App_Code
{
    public class LdapAuthentication
    {
        private String _path;
        private String _filterAttribute;
        private String _ErrorMessage;

        public String ErrorMessage
        {
            get { return _ErrorMessage; }
        }

        public LdapAuthentication(String path)
        {
            _path = path;
        }

        public bool IsAuthenticated(String domain, String username, String pwd)
        {
            _ErrorMessage = "";
            Boolean retresult = false;
            String domainAndUsername = domain + @"\" + username;
            //DirectoryEntry entry = new DirectoryEntry(_path, domainAndUsername, pwd);
            DirectoryEntry entry = new DirectoryEntry(_path, username, pwd);
            try
            {	//Bind to the native AdsObject to force authentication.			
                Object obj = entry.NativeObject;

                DirectorySearcher search = new DirectorySearcher(entry);

                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();

                if (null != result)
                {
                    retresult = true;
                }

                //Update the new path to the user in the directory.
                //_path = result.Path;
                //_filterAttribute = (String)result.Properties["cn"][0];
            }
            catch (Exception ex)
            {
                _ErrorMessage = ex.Message;
            }

            return retresult;
        }

        public String GetGroups()
        {
            _ErrorMessage = "";
            DirectorySearcher search = new DirectorySearcher(_path);
            search.Filter = "(cn=" + _filterAttribute + ")";
            search.PropertiesToLoad.Add("memberOf");
            StringBuilder groupNames = new StringBuilder();

            try
            {
                SearchResult result = search.FindOne();

                int propertyCount = result.Properties["memberOf"].Count;

                String dn;
                int equalsIndex, commaIndex;

                for (int propertyCounter = 0; propertyCounter < propertyCount; propertyCounter++)
                {
                    dn = (String)result.Properties["memberOf"][propertyCounter];

                    equalsIndex = dn.IndexOf("=", 1);
                    commaIndex = dn.IndexOf(",", 1);
                    if (-1 == equalsIndex)
                    {
                        return null;
                    }

                    groupNames.Append(dn.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1));
                    groupNames.Append("|");

                }
            }
            catch (Exception ex)
            {
                _ErrorMessage = ex.Message;
            }
            return groupNames.ToString();
        }

        public static string GetProperty(SearchResult searchResult, string PropertyName)
        {
            if (searchResult.Properties.Contains(PropertyName))
            {
                return searchResult.Properties[PropertyName][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public Dictionary<string, string> GetUseDetails(String domain, String username, String pwd)
        {
            Dictionary<string, string> Res = new Dictionary<string, string>();
            String domainAndUsername = domain + @"\" + username;
            DirectoryEntry entry = new DirectoryEntry(_path, domainAndUsername, pwd);
            DirectorySearcher Dsearch = new DirectorySearcher(entry);
            Dsearch.Filter = "(SAMAccountName=" + username + ")";
            Dsearch.PropertiesToLoad.Add("cn");
            Dsearch.PropertiesToLoad.Add("userPrincipalName");
            Dsearch.PropertiesToLoad.Add("mail");
            Dsearch.PropertiesToLoad.Add("mobile");
            Dsearch.PropertiesToLoad.Add("physicalDeliveryOfficeName");
            SearchResult result = Dsearch.FindOne();
            if (result != null)
            {
                Res.Add("Name", GetProperty(result, "cn"));
                Res.Add("EmployeeID", GetProperty(result, "userPrincipalName"));
                Res.Add("Email", GetProperty(result, "mail"));
                Res.Add("Mobile", GetProperty(result, "mobile"));
                Res.Add("Location", GetProperty(result, "physicalDeliveryOfficeName"));
            }

            return Res;
        }

    }
}