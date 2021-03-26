// ************************************************************************
// reportfunction (计支宝基础函数)
// 2018/8/13	
// 作者∶hbdai    E-mail: 413130678@qq.com
// ************************************************************************
 
var Ht_rptType = "";
var Ht_rptId = ""; 
var ExeclName = "";
var Rpt_order = "";//报表序号 king.zhang 2019-5-6
var Rpt_name = "";//报表名称
var Cont_id_global = "";
var MsrReport_loadmsring = "0";
var Selectionpicname = "";
var reportPeriodid = "";

 

//打印预览
function HtmnuFilePrintPreview_click() {
    var sheetCount = spread.sheets.length; //总表页数
    var selectWorksheetsPageID = spread.getActiveSheetIndex();//当前活动表页
    if (sheetCount > 1) {
        selectWorksheetsPageID = confirm("当前表页有多页，点击确定预览当前页，点击取消预览全部页") ? selectWorksheetsPageID : -1;
    }
    if (MsrReport_loadmsring == 1) {
        $.messager.alert("提示", "数据正在加载中!!", "info"); return;
    } 
    MsrReport_loadmsring = 1;
    $.post("/Report/DownloadPdf", {
        sheetsPageID: selectWorksheetsPageID,
        Ex_Name: ExeclName,
        RptId: Ht_rptId,
        periodid: reportPeriodid,
        random: Math.random()
    }, function (result, status) {
        if (status == "success") {
            MsrReport_loadmsring = 0;
            window.open(result);
        }
        else {
            MsrReport_loadmsring = 0;
            alert("请稍后预览");
        }
    });
}
//模板-打印预览
function HtmnuFilePrintPreview_MB() {
    if (MsrReport_loadmsring == 1) {
        $.messager.alert("提示", "数据正在加载中!!", "info"); return;
    }
    MsrReport_loadmsring = 1;
    $.post("/Report/DownloadPdf", {
        sheetsPageID: selectWorksheetsPageID,
        Ex_Name: "",
        RptId: Ht_rptId,
        random: Math.random()
    }, function (result, status) {
        if (status == "success") {
           MsrReport_loadmsring = 0;
           window.open(result);
        }
        else {
            MsrReport_loadmsring = 0;
            alert("请稍后预览");
        }
    });
}

//导入Excel文件
function HtmnuFileImportExcel_click() {
    return $("#ReportTemplat_execlCheck").click();
}
function HtmnuFileImportExcel_click1(input) {
    var oFReader = new FileReader(); 
    var fd = new FormData();
    fd.append("fileToUpload", document.getElementById('ReportTemplat_execlCheck').files[0]); //这是获取上传的文件
    $.ajax({
        url: "/Report/mnuFileImportExcel?RptId=" + Ht_rptId + "&r=" + Math.random(),
        type: "POST",
        data: fd,
        processData: false,  // 不处理数据
        contentType: false,   // 不设置内容类型
        success: function (result, status) {
            document.getElementById('ReportTemplat_execlCheck').value = '';            
            if (status == "success") {
                var _tempJson = JSON.parse(result);
                selectWorksheetsPageID = 0;
                var fileType = ""; //指定mime
                var myFile = createFile(_tempJson.Data, fileType);
                excelIo.open(myFile, function (json) {
                    var workbookObj = json;
                    spread.fromJSON(workbookObj);
                }, function (e) {

                    console.log(e);
                });
            } else $.messager.alert("提示", "数据加载失败", "info");
           
        }
    });
}

//导出Excel文件
function HtFileExportExcel_click() {
    if (ExeclName != '') {
        //window.open('/upload/cache/' + ExeclName);//直接下载当前文件，带签名等
        $.post("/Report/DownloadFileExportExcelx", {
            filenamex: ExeclName,
            RptId: Ht_rptId
        }, function (result, status) {
          if (status == "success") {
            window.open(result);
          }
        });
    }
    else {
        HtFileExportExcelMb_click();
    }
}

function getFileAndDownload(fileName, url) {
    var x = new XMLHttpRequest();
    x.open("GET", url, true);
    x.responseType = 'blob';
    x.onload = function (e) {
        var blob = x.response;
        if ('msSaveOrOpenBlob' in navigator) {//IE导出
            window.navigator.msSaveOrOpenBlob(blob, fileName);
        }
        else {
            var a = document.createElement('a');
            a.download = fileName;
            a.href = URL.createObjectURL(blob);
            $("body").append(a);
            a.click();
            $(a).remove();
        }
    };
    x.send();
}

//导出Excel文件
function HtFileExportExcelMb_click() {
    $.post("/Report/DownloadFileExportExcel", {
        sheetsPageID: selectWorksheetsPageID,
        RptId: Ht_rptId,
        random: Math.random()
    }, function (result, status) {
        if (status == "success") {
            window.open(result);
        }
    });
}

function createFile(urlData, fileType) {
  debugger;
    var bytes = window.atob(urlData),
         n = bytes.length,
         u8arr = new Uint8Array(n);
    while (n--) {
        u8arr[n] = bytes.charCodeAt(n);
    }
    return new Blob([u8arr], { type: fileType });
}
 

function initSpread(spread) {
    reportPeriodid = "";Ht_rptId = "";
    var spreadNS = GC.Spread.Sheets;   
    spreadNS.AutoResize = true; 
};
