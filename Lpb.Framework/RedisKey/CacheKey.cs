using System;

namespace RedisKey
{
    /// <summary>
    /// 微服务缓存Key
    /// </summary>
    public class CacheKeyService
    {
        /// <summary>
        /// 自助模式设备忙，key:订单编号,value:bool
        /// </summary>
        public const string DeviceBusy = "DeviceBusy";
        /// <summary>
        /// 自助模式设备占用，key:设备Iot的Id,value:bool
        /// </summary>
        public const string DeviceRunning = "DeviceRunning";
        /// <summary>
        /// 自助模式二维码时间戳，key:设备Iot的Id,value:bool
        /// </summary>
        public const string QrCodeTimestamp = "QrCodeTimestamp";
        /// <summary>
        /// 自助模式设备初始化状态，key:设备Iot的Id,value:bool
        /// </summary>
        public const string DeviceInitSuccess = "DeviceInitSuccess";
        /// <summary>
        /// 自助模式设备信息，key:设备Iot的Id,value:DeviceInit
        /// </summary>
        public const string DeviceInfo = "DeviceInfo";
        /// <summary>
        /// 自助模式设备分配，key:设备Iot的Id,value:OrganizationDeviceListDto
        /// </summary>
        public const string DeviceOrganization = "DeviceOrganization";


        /// <summary>
        /// 网关Token，key:ClientId + userId,value:string
        /// </summary>
        public const string GatewayToken = "GatewayToken";
        /// <summary>
        /// 软件登陆的设备唯一Id，key:ClientId + userId,value:deviceUUID List
        /// </summary>
        public const string DeviceUser = "DeviceUser";
        /// <summary>
        /// 软件在单一固定设备上的Token，key:ClientId + userId + deviceUUID,value:stringList
        /// </summary>
        public const string DeviceToken = "DeviceToken";
        /// <summary>
        /// 网关Token黑名单，key:ClientId + userId,value:string
        /// </summary>
        public const string BlacklistToken = "BlacklistToken";
        /// <summary>
        /// 刷新token，key:ClientId + userId,value:string
        /// </summary>
        public const string RefreshToken = "RefreshToken";
        /// <summary>
        /// 关注微信公众号后的微信用户信息，key:unionid,value:WeixinMpSubscribe
        /// </summary>
        public const string WeixinMp = "WeixinMp";
        /// <summary>
        /// App版本信息，key:定值"App",value:AppVision
        /// </summary>
        public const string AppVision = "AppVision";
        /// <summary>
        /// 后台管理中操作医生用户角色信息，key:userId,value:AppVision
        /// </summary>
        public const string Operator = "Operator";
        /// <summary>
        /// 阿里云OSS(sts)，key:sts,value:AliStsResponseModel
        /// </summary>
        public const string AliyunSts = "AliyunSts";
        /// <summary>
        /// 小程序表单，key:formId,value:FormIdSaveInput
        /// </summary>
        public const string WxOpenForm = "WxOpenForms";
        /// <summary>
        /// 手机验证码，key:手机号码,value:string
        /// </summary>
        public const string PhoneAuthCode = "PhoneAuthCode";
        /// <summary>
        /// 定时作业，key:作业类型名称,value:作业的唯一标识
        /// </summary>
        public const string QuartzJob = "QuartzJob";


        public const string MessagetemplatesCache = "MessagetemplatesCache";
    }

    public class CacheKeyDataBase
    {
        /// <summary>
        /// 检查数据（检查项+就诊人），key:检查记录,value:结果数据
        /// </summary>
        public const string ExamData = "ExamData";
        /// <summary>
        /// 检查数据（检查项+就诊人），key:检查记录,value:结果数据
        /// </summary>
        public const string ExamFirstData = "ExamFirstData";


        /// <summary>
        /// 就诊人信息，key:PatientId,value:Patient
        /// </summary>
        public const string Patient = "Patient";
        /// <summary>
        /// 就诊人 扩展信息，key:PatientId,value:PatientExtend
        /// </summary>
        public const string PatientExtend = "PatientExtend";
        /// <summary>
        /// 用户拥有的就诊人，key:UserId,value:Patient_List
        /// </summary>
        public const string UserPatients = "UserPatients";

        /// <summary>
        /// 检查记录，key:ExamRecordId,value:ExamRecord
        /// </summary>
        public const string ExamRecord = "ExamRecord";
        /// <summary>
        /// 就诊人某年拥有的就诊记录，key:PatientId,value:ExamRecord_List
        /// </summary>
        public const string PatientExamRecordAll = "PatientExamRecordAll";
        /// <summary>
        /// 就诊人某年拥有的就诊记录，key:PatientId+Year,value:ExamRecord_List
        /// </summary>
        public const string PatientExamRecords = "PatientExamRecords";
        /// <summary>
        /// 就诊人拥有的就诊年列表，key:PatientId,value:int_List
        /// </summary>
        public const string PatientExamRecordYears = "PatientExamRecordYears";


        /// <summary>
        /// 检查单,key:CheckItemId,value:CheckItem
        /// </summary>
        public const string CheckItem = "CheckItem";
        /// <summary>
        /// 检查记录对应的检查单,key:ExamRecordId,value:CheckItem_list
        /// </summary>
        public const string ExamRecordCheckItems = "ExamRecordCheckItems";
        /// <summary>
        /// 订单编号对应的检查单,key:OrderNumber,value:CheckItem_list
        /// </summary>
        public const string OrderNumberCheckItems = "OrderNumberCheckItems";



        /// <summary>
        /// App用户信息
        /// </summary>
        public const string AppUserInfo = "AppUserInfo";
        /// <summary>
        /// App用户最后登录方式
        /// </summary>
        public const string AppUserLoginRecord = "AppUserLoginRecord";



        /// <summary>
        /// 租户结算信息
        /// </summary>
        public const string TenantSettlement = "TenantSettlement";

    }

    public class CacheKeyBackStage
    {
        /// <summary>
        /// Token 缓存名称
        /// </summary>
        public const string AccessToken = "AccessToken";
        public const string RefreshToken = "RefreshToken";

        /// <summary>
        /// 阿里云OSS配置 缓存名称
        /// </summary>
        public const string AliyunOssConfigCache = "AliyunOssConfigCache";
        public const string AliyunOssConfigCache_AccessKeyId = "AccessKeyId";
        public const string AliyunOssConfigCache_AccessKeySecret = "AccessKeySecret";
        public const string AliyunOssConfigCache_Region = "Region";
        public const string AliyunOssConfigCache_Bucket = "Bucket";
        public const string AliyunOssConfigCache_DoesObjectExist = "DoesObjectExist";

        /// <summary>
        /// Resource资源服务 缓存名称
        /// </summary>
        public const string ResourceServicesCache = "ResourceServicesCache";
        //获取全部资讯类型列表
        public const string ResourceServicesCache_GetAllInfomationType = "GetAllInfomationType";

        /// <summary>
        /// 产品信息 缓存名称
        /// </summary>
        public const string ProductInfo = "ProductInfos";
        //获取全部产品信息列表
        public const string ProductInfo_GetAllProductInfos = "GetAllProductInfos";

        /// <summary>
        /// 
        /// </summary>
        public const string PatientsExcelUpLoadCache = "PatientsExcelUpLoadCache";
        public const string PatientsExcelErrorCache = "PatientsExcelErrorCache";
        public const string PatientsExcelUpLoadCache_ExcelDataTable = "ExcelDataTable:";


    }

    public class CacheKeyHuanxin
    {
        /// <summary>
        /// Token 缓存名称
        /// </summary>
        public const string AccessToken = "HuanxinAccessToken";

    }
}
