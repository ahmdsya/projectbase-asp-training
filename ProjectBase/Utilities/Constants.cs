using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Constants
{
    public const string HRSYSTEM_ADDRESS = /*"http://10.1.1.150:8181";*/ "http://10.1.1.111";
    public const string _ERROR = "_ERROR";
    public const string _HOME = "HOME";
    public const string TRANS_RESULT = "TRANS_RESULT";

    public const string LOOKUP_BY_LOCATION_GROUP_WB_LOCATION = "WB_LOCATION";

    public const string LOOKUP_GROUP_MATERIAL_TYPE = "MATERIAL_TYPE";
    public const string LOOKUP_GROUP_MATERIAL_GROUP = "MATERIAL_GROUP";
    public const string LOOKUP_GROUP_UOM = "UOM";
    public const string LOOKUP_GROUP_PRODUCTION_CATEGORY = "PRODUCTION_CATEGORY";
    public const string LOOKUP_GROUP_SOUNDING_TOOL = "SOUNDING_TOOL";
    public const string LOOKUP_GROUP_SHIFT_NO = "SHIFT_NO";
    public const string LOOKUP_GROUP_OIL_TYPE = "OIL_TYPE";
    public const string LOOKUP_GROUP_QC_TYPE = "QC_TYPE";

    public const string TYPE_LOADING = "Loading";
    public const string TYPE_UNLOADING = "Unloading";

    public const string CLAIM_EMPLOYEE_NUMBER = "CLAIM_EMPLOYEE_NUMBER";
    public const string CLAIM_ID = "CLAIM_ID";
    public const string CLAIM_NAME = "CLAIM_NAME";
    public const string CLAIM_FULLNAME = "CLAIM_FULLNAME";
    public const string CLAIM_EMAIL = "CLAIM_EMAIL";
    public const string CLAIM_IS_ADMIN = "CLAIM_IS_ADMIN";
    public const string CLAIM_COUNTRY = "CLAIM_COUNTRY";
    public const string CLAIM_PROVINCE = "CLAIM_PROVINCE";
    public const string CLAIM_CITY = "CLAIM_CITY";
    public const string CLAIM_PHONE_NUMBER = "CLAIM_PHONE_NUMBER";
    public const string CLAIM_LANGUAGE_PREFERENCES = "CLAIM_LANGUAGE_PREFERENCES";
    public const string CLAIM_MENU = "CLAIM_MENU";
    public const string CLAIM_LOGIN_BY_AD = "CLAIM_LOGIN_BY_AD";

    public const string AD_DOMAIN = "wil.local";
    public const string AD_SA_USER = "id_ad_integrator";
    public const string AD_SA_PASSWORD = "askl1290!@#";

    public const string USER_STATUS_ACTIVE = "Active";
    public const string USER_STATUS_INACTIVE = "Inactive";

    public const string FORM_MODE_CREATE = "Create";
    public const string FORM_MODE_EDIT = "Edit";
    public const string FORM_MODE_VIEW = "View";
    public const string FORM_MODE_DELETE = "Delete";
    public const string FORM_MODE_PRINT = "Print";
    public const string FORM_MODE_COPY = "Copy";
    public const string FORM_MODE_UNCHANGED = "Unchanged";

    public const string ERROR_TAG = "[Error]";

    //Settings
    public const string SETTING_VERSION = "SETTING_VERSION";


    public const string OK = "OK";
    public const string DATE_FORMAT_CLIENT = "dd/MM/yyyy";
    public static string[] dataKey = new string[14]
    {
            "emp", "name", "email", "company", "area", "sub_area", "grade",
            "unit", "dept", "pay_area", "deptname", "ptname", "telp", "birth_date"
        //,"phone_no", "homebase", "loker", "ktp", "ktp_name", "passport", "passport_name", "gender"
    };

    //public const string BG_STATUS_HOLD = "#fff";
    //public const string BG_STATUS_APPROVED = "#55c3f9"; //"#03A9FA";
    //public const string BG_STATUS_REJECT = "#ff9776fa"; //"#FF5722";
    //public const string BG_STATUS_ISSUED = "#FFEB3B";
    //public const string BG_STATUS_AGREED = "#FFC107";
    //public const string BG_STATUS_PROCESSED = "#3bfb3b"; //"#00FF00";

    public const string BG_STATUS_HOLD = "#fff";
    public const string BG_STATUS_APPROVED = "rgb(189, 136, 111)";
    public const string BG_STATUS_REJECT = "rgba(200, 106, 202, 0.98)";
    public const string BG_STATUS_ISSUED = "rgb(224, 159, 228)";
    public const string BG_STATUS_HEAD_APPROVAL = "rgb(230, 221, 111)";
    public const string BG_STATUS_AGREED = "rgb(113, 214, 205)";
    public const string BG_STATUS_PROCESSED = "rgb(167, 224, 167)";
    public static string[] UPLOAD_FILE_TYPES = new string[]
    {
        "image/gif",
        "image/jpeg",
        "image/pjpeg",
        "image/png",
        "application/vnd.ms-outlook",
        "application/pdf"
    };
}

public class TransResult
{
    public string message { get; set; }
    public long Id { get; set; }
    public string Ref_No { get; set; }
    public string Action { get; set; }
    public string Controller { get; set; }
}

public class ColumnClass
{
    public string data { get; set; }
    public string name { get; set; }
    public Nullable<bool> orderable { get; set; }
    public Nullable<bool> visible { get; set; }
    public Nullable<bool> searchable { get; set; }
}

public class OrderClass
{
    public int column { get; set; }
    public string dir { get; set; }
}

public class ConnectionClass
{
    public string server { get; set; }
    public string database { get; set; }
    public string user { get; set; }
    public string password { get; set; }
}