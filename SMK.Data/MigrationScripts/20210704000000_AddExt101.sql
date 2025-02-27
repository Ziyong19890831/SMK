-- 新增選單 資料管理
INSERT INTO [SMKWEB].[dbo].[Privilege]([Id], [ParentId], [Name], [Sort], [Type], [ControllerName], [ActionName], [QueryParams], [Remark], [CreatedAt], [UpdatedAt], [UpdatedBy]) VALUES(N'7700c2b7-431d-4a3c-aa89-ce60c4168f17', NULL, N'資料管理', 1, N'Node', NULL, NULL, NULL, NULL, '20201105 00:42:31.721', NULL, NULL);
INSERT INTO [SMKWEB].[dbo].[RolePrivilegeMapping]([Id], [RoleId], [PrivilegeId], [EnableEntry], CreatedAt) VALUES(N'fc886852-0558-4f16-a167-0f61fa669134', N'00000000-0000-0000-0000-000000000000', N'7700c2b7-431d-4a3c-aa89-ce60c4168f17', 1, '20201105 00:42:31.721');

-- 新增選單 資料管理>資料報表/輸出
INSERT INTO Privilege (Id,ParentId,Name,Sort,Type,ControllerName,ActionName,QueryParams,Remark) VALUES('acee667a-3661-4a96-8c10-744c5e4931b9','7700c2b7-431d-4a3c-aa89-ce60c4168f17','資料報表/輸出',1,'Link','DataExport','Index',null,null)
INSERT INTO RolePrivilegeMapping (Id,RoleId,PrivilegeId,EnableEntry) VALUES('b3a78bd7-3f77-419e-bf4c-32adc546ff09','00000000-0000-0000-0000-000000000000','acee667a-3661-4a96-8c10-744c5e4931b9',1)

-- 新增選單 資料管理>個案資料
INSERT INTO Privilege (Id,ParentId,Name,Sort,Type,ControllerName,ActionName,QueryParams,Remark) VALUES('a19221fe-5eb7-48b6-8f09-498a0a74559d','7700c2b7-431d-4a3c-aa89-ce60c4168f17','個案資料',1,'Link','DataQuery','Index',null,null);
INSERT INTO RolePrivilegeMapping (Id,RoleId,PrivilegeId,EnableEntry,CreatedAt) VALUES('8971c382-5162-4940-871f-8454eabbb132','00000000-0000-0000-0000-000000000000','a19221fe-5eb7-48b6-8f09-498a0a74559d',1,'2021-07-12');


CREATE NONCLUSTERED INDEX [IX_iniOpDtl_tran_date]
	ON [dbo].[iniOpDtl]([tran_date])

CREATE NONCLUSTERED INDEX [IX_iniOpDtl_birthday]
	ON [dbo].[iniOpDtl]([birthday]);


-- 新增選單 檔案上傳>健保資料上傳結果
INSERT INTO Privilege (Id,ParentId,Name,Sort,Type,ControllerName,ActionName,QueryParams,Remark,CreatedAt,UpdatedAt,UpdatedBy) VALUES('eb8c0310-9234-48e0-932e-4c26e0d254f9','b0a2f58c-2870-4997-a9c9-633dede96018','健保資料上傳結果',3,'Link','IniFileInCtrl','Index',null,null,'2020-11-05 00:42:31.7214525',null,null);
INSERT INTO RolePrivilegeMapping (Id,RoleId,PrivilegeId,EnableEntry,CreatedAt) VALUES('a1d5a509-9784-42ef-bb2a-b670574eb4b4','00000000-0000-0000-0000-000000000000','eb8c0310-9234-48e0-932e-4c26e0d254f9',1,'2021-07-12 00:00:00.0');

-- 新增選單 檔案上傳>戒菸率資料
INSERT INTO Privilege (Id,ParentId,Name,Sort,Type,ControllerName,ActionName,QueryParams,Remark,CreatedAt,UpdatedAt,UpdatedBy) VALUES('4601bd1a-8183-4fe5-83fa-a51b9cd3caa9','b0a2f58c-2870-4997-a9c9-633dede96018','戒菸率資料',3,'Link','QuitDataAll','Index',null,null,'2020-11-05 00:42:31.7214525',null,null);
INSERT INTO RolePrivilegeMapping (Id,RoleId,PrivilegeId,EnableEntry,CreatedAt) VALUES('188a438b-efe6-4089-8531-a1ab033513a3','00000000-0000-0000-0000-000000000000','4601bd1a-8183-4fe5-83fa-a51b9cd3caa9',1,'2021-07-12 00:00:00.0');


CREATE INDEX IX_MhbtQsData_HospID
    ON MhbtQsData (HospID,[ID],Birthday,CureStage,ExamYear,Cure_Type);
