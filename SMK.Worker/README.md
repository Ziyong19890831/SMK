# SMK Worker Service

## 安裝方式

將 workerPub 目錄下所有檔案 copy 到正式環境
```
dotnet publish -c Release -o ../WorkerPub
```

## 修改設定 .\appsettings.json

修改 FileSystem 區段的設定調整為檔案上傳的目錄
```json
{
  "FileWatchService": {
    "IniDrDtlRootPath": "D:\\Projects\\hpa\\SMK\\SMK.Web\\wwwroot\\DataImport\\iniDrDtlTxt",
    "IniDrOrdRootPath": "D:\\Projects\\hpa\\SMK\\SMK.Web\\wwwroot\\DataImport\\iniDrOrdTxt",
    "IniOpDtlRootPath": "D:\\Projects\\hpa\\SMK\\SMK.Web\\wwwroot\\DataImport\\iniOpDtlTxt",
    "IniOpOrdRootPath": "D:\\Projects\\hpa\\SMK\\SMK.Web\\wwwroot\\DataImport\\iniOpOrdTxt",
    "QuitDataAllRootPath": "D:\\Projects\\hpa\\SMK\\SMK.Web\\wwwroot\\DataImport\\QuitDataAllTxt",
    "CstAgentPatientRootPath": "D:\\Projects\\hpa\\SMK\\SMK.Web\\wwwroot\\DataImport\\AgentPatientTxt",
    "CstQsCureRootPath": "D:\\Projects\\hpa\\SMK\\SMK.Web\\wwwroot\\DataImport\\QsCureTxt",
    "CstQsDataRootPath": "D:\\Projects\\hpa\\SMK\\SMK.Web\\wwwroot\\DataImport\\QsDataTxt",
    "CstQsStateRootPath": "D:\\Projects\\hpa\\SMK\\SMK.Web\\wwwroot\\DataImport\\QsStateTxt",
    "DataImportRootPath": "D:\\Projects\\hpa\\SMK\\SMK.Web\\wwwroot\\DataImport\\temp"
    "DataExportRootPath": "D:\\Projects\\hpa\\SMK\\SMK.Web\\wwwroot\\DataExport\\"
  }
}
```

## 啟動服務，此種啟動方式在登入後，服務會立即停止，正式環境建議使用服務方式
```
.\SMK.Worker.exe
```

以下指令都需要用 administrator 權限

## 註冊服務
```
sc.exe create SMKWorkerService binPath=D:\WorkerPub\SMK.Worker.exe
```

## 查看服務狀態
```
sc.exe query SMKWorkerService
```

## 啟動服務指令
```
sc.exe start SMKWorkerService
```

## 停用、刪除指令
```
sc.exe stop SMKWorkerService
sc.exe delete SMKWorkerService
```
