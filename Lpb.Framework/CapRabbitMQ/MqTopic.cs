namespace CapRabbitMQ
{
    public static class MqTopic
    {
        /////////////////////////////////数据服务器发送////////////////////////////////////

        /// <summary>
        /// 检查失败，数据保存失败
        /// 参数：ExamDataSaveResult
        /// </summary>
        public const string DataServer_Device_ExamDataSaveResult = "DataServer.Device.ExamDataSaveResult";

        /////////////////////////////////支付服务器发送////////////////////////////////////

        /// <summary>
        /// 支付状态发送，自助筛查（启动设备，更新检查单状态），视光中心（更新检查单状态）
        /// 参数：UserPayResult
        /// </summary>
        public const string PayServer_PayResult = "PayServer.PayResult";

        /////////////////////////////////IoT服务器发送////////////////////////////////////

        /// <summary>
        /// 延时关闭订单
        /// 参数：string
        /// </summary>
        public const string IoT_CloseOrderDelay = "IoT.CloseOrderDelay";


        /////////////////////////////////后台服务器发送////////////////////////////////////
        /// <summary>
        /// 更新 组织单元名称
        /// 参数：Organization
        /// </summary>
        public const string BackStage_UpdateOrganization = "BackStage.UpdateOrganization";
        /// <summary>
        /// 更新 租户名称
        /// 参数：Organization
        /// </summary>
        public const string BackStage_UpdateTenant = "BackStage.UpdateTenant";
        /// <summary>
        /// 更新 用户名称
        /// 参数：Organization
        /// </summary>
        public const string BackStage_UpdateUser = "BackStage.UpdateUser";
        /// <summary>
        /// 更新 App版本信息
        /// 参数：AppVision
        /// </summary>
        public const string BackStage_UpdateAppVision = "BackStage.UpdateAppVision";


        /////////////////////////////////Customer服务器发送////////////////////////////////////
        /// <summary>
        /// 更新 就诊人信息（同步修改检查记录）
        /// 参数：List ActivityCheckItem
        /// </summary>
        public const string Customer_UpdatePatientInfo = "Customer.UpdatePatientInfo";
        /// <summary>
        /// 开单后，检查全部检查完成
        /// 参数：ExamRecordListDto
        /// </summary>
        public const string Customer_ExamRecoedExamed = "Customer.ExamRecoedAllExamed";


        /////////////////////////////////Diagnose服务器发送////////////////////////////////////
        /// <summary>
        /// 诊断完成后，提醒App用户查看医生诊断
        /// 参数：ExamRecordListDto
        /// </summary>
        public const string Diagnose_ExamRecoedDiagnosed = "Diagnose.ExamRecoedDiagnosed";


        /////////////////////////////////Doctor服务器发送////////////////////////////////////
        /// <summary>
        /// 医生接诊用户App提交的诊断申请
        /// 参数：ExamVisitListDto
        /// </summary>
        public const string Doctor_ExamVisitAgree = "Doctor.ExamVisitAgree";


        /////////////////////////////////User服务更新医生姓名////////////////////////////////////
        /// <summary>
        /// 更新 医生信息
        /// 参数：UpdateDoctor
        /// </summary>
        public const string Doctor_UpdateDoctor = "Doctor.UpdateDoctor";


        /////////////////////////////////消息队列心跳////////////////////////////////////
        /// <summary>
        /// 消息队列心跳
        /// 参数：Marketing，BackStage，DataServer，Weixin
        /// </summary>
        public const string Lpb_Heartbeat = "Lpb.Heartbeat";


        /////////////////////////////////微信服务器发送////////////////////////////////////
        /// <summary>
        /// 微信订阅（关注）事件
        /// 参数：WeixinMpSubscribe
        /// </summary>
        public const string Lpb_Weixin_Subscribe = "Lpb.Weixin.Subscribe";


        /////////////////////////////////服务端（需要发送消息至APP消息列表的）发送////////////////////////////////////
        /// <summary>
        /// 发送消息至消息列表
        /// 参数：CreateMessageInput
        /// </summary>
        public const string Lpb_App_UserMessage = "Lpb.App.UserMessage";

    }
}
