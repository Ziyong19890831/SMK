# SMK.Web

## Code First注意事項

### Database Migration
    若要異動 db schema 請一律從 entity 異動，流程如下，以利同步

### Visual Studio 起始專案設為 SMK.Data,  開始 conosle 上方 default project 設為 SMK.Data

* enttiy 異動後 migration

    * 新增
    `> Add-Migration {key-a-name-for-this-change-here}`
    
    * 產出異動 script
    `> Script-Migration  {from-migration-name} {to-migration-name}`
    
    * 更新 db
    `> Update-Database`

   
* View 新增異動

    請自行建立 新增 or 異動的 script 放在 MigrationScriptis/View 底下


* 新增資料

    可以將 scripit 放在 MigrationScriptis/Seed 底下

## 專案初始化步驟

1. solution`還原Nuget套件`
2. `libman.json`還原`wwwroot/lib`

## 注意事項
- 日期以民國年呈現
- 醫事機構關鍵字採autocomplete
- 查詢增加一個下載檔案的功能

## 舊table

| Table          | Table名稱    |
| -------------- | ------------ |
| GenEmpData     | 人員名單     |
| GenProgramData | 程式清單     |
| GenBranch      | 分局別       |
| GenHospCont    | 特約         |
| GenEndReason   | 終止原因     |
| GenSMKContract | 戒菸合約     |
| GenPrsnType    | 醫事人員類別 |
| GenLicenceType | 證書類別     |
| GenSpecial     | 專科別       |
| HospBasic      | 醫事機構     |
| PrsnBasic      | 醫事人員     |

## 新Table 

| Table                 | Table名稱          |
| --------------------- | ------------------ |
| GenEmpData            | 人員名單           |
| __EFMigrationsHistory | EF version         |
| AuditLog              | 資料庫操作紀錄     |
| Privilege             | 程式清單           |
| Role                  | 權限表             |
| RoleEmpMapping        | 人員權限對應表     |
| RolePrivilegeMapping  | 權限程式清單對應表 |
| PrsnContract          | 醫事人員合約       |
| GenEmpData            | 人員名單           |
| GenBranch             | 分局別             |
| GenEndReason          | 終止原因           |
| GenHospCont           | 特約               |
| HospBscAll            | 所有醫事機構       |
| GenLicenceType        | 證書清單           |
| GenPrsnType           | 醫事人員類別       |
| GenSMKContract        | 戒菸合約           |
| GenSpecial            | 專科別             |
| HospContract          | 機構合約           |
| HospBasic             | 合約機構           |
| PrsnBasic             | 醫事人員清單       |