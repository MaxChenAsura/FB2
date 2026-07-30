USE [FB2DB]
GO

/****** Object:  StoredProcedure [dbo].[SP_S_ASSESS_DEP20_NOTIFY_MAIL]    Script Date: 2026/7/29 下午 04:08:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Max 
-- Create date: 2021/10/26
-- Update date: 2021/10/26
-- Description:	考核表部長通知簽核作業- 考核年度、考核類別, 直屬部門,  使用者id,FunctionID
-- =============================================

CREATE procedure   [dbo].[SP_S_FASSESS_DEP20_NOTIFY_MAIL]
	@ASSESS_YEAR varchar(4), 
	@ASSESS_TYPE varchar(1), 
	@DEPT_NO varchar(7), 
	@USERID varchar(20),
	@FUNCID varchar(30)
as
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;
	--錯誤訊息
	DECLARE @pRowCount INT =0;
	DECLARE @pErr INT = 0;
	DECLARE @pErrMsg VARCHAR(1000) = NULL;
	
	DECLARE @proc_id_previous VARCHAR(60) = 'SP_S_FASSESS_DEP20_NOTIFY_MAIL';
	DECLARE @proc_id VARCHAR(60) = 'SP_S_FASSESS_DEP20_NOTIFY_MAIL';
	DECLARE @proc_desc NVARCHAR(120) = '考核表部長通知簽核作業';
	DECLARE @proc_scheduling VARCHAR(30) = 'CALL';
	DECLARE @proc_log NVARCHAR(600) = NULL;
	DECLARE @proc_status VARCHAR(1) = NULL;
	DECLARE @proc_y_cnt INT;

	DECLARE @non_sign_count int;
	DECLARE @non_resign_count int;
	DECLARE @mail_flag varchar(1);
	DECLARE @dept_no_20 varchar(7);
	DECLARE @ori_score_final varchar(2);
	DECLARE @is_out varchar(1);
	DECLARE @ori_comments  varchar(200);
	DECLARE @new_comments  varchar(200);
	DECLARE @level_cd varchar(3);
	DECLARE @ws_cd varchar(1);
	DECLARE @ma_type varchar(1);
	DECLARE @ma_emp_id varchar(5);
	DECLARE @assess_type_desc varchar(20);
	DECLARE @deadline datetime;
	DECLARE @week varchar(10);
	DECLARE @month varchar(2);
	DECLARE @day varchar(2);
	DECLARE @company_email varchar(60);
	DECLARE @head_emp_id  varchar(5);
	--mail檔的資料
	declare 
	@MAIL_SUBJECT	NVARCHAR(100)='' --主旨
   ,@MAIL_CONTENT	NVARCHAR(Max)=''  --信件內容
   ,@MAILTO			VARCHAR(36)=''--mail通知人
   ,@MAIL_DESC		NVARCHAR(20)=''--mail說明
	;

	--取得該直部門的部處下尚未簽核的部門數量
	SELECT @non_sign_count=count(*) from TB_S_M_FOREIGN_DIRECTOR_H H WHERE H.DEPT_NO in
	(
	SELECT SC.DEPT_NO
	FROM TB_H_R_DEPT_DATA_AD SA join
		 TB_S_M_FOREIGN_DEPT_LEVEL SB on SA.DEPT_NO_20=SB.DEPT_NO and SB.DEPT_LEVEL>='20'  join
		 TB_S_M_FOREIGN_DEPT_LEVEL SC on SUBSTRING(SC.LEVEL_RATE,1,len(SB.LEVEL_RATE))=SB.LEVEL_RATE  and SB.ASSESS_YEAR=SC.ASSESS_YEAR and SB.ASSESS_TYPE=SC.ASSESS_TYPE
	WHERE SA.DEPT_NO=@DEPT_NO and SB.ASSESS_YEAR=H.ASSESS_YEAR and SB.ASSESS_TYPE=H.ASSESS_TYPE
	)
	and H.ASSESS_YEAR=@ASSESS_YEAR and H.ASSESS_TYPE=@ASSESS_TYPE and isnull(SIGN_YN,'')<>'Y'
	
	
	--檢查是否已送出通知信
	SELECT distinct @dept_no_20=A.DEPT_NO_20, @mail_flag=isnull(B.MAIL_FLAG,'N') ,@head_emp_id=B.emp_id
	FROM TB_H_R_DEPT_DATA_AD A join
	    TB_S_M_FOREIGN_DEP20_UP_SIGN B on A.DEPT_NO_20 = B.DEPT_NO 
	WHERE A.DEPT_NO=@DEPT_NO and B.ASSESS_YEAR=@ASSESS_YEAR and B.ASSESS_TYPE=@ASSESS_TYPE and B.DEPT_LEVEL='20' ;
	
	--取得尚未覆核子部門數量
	select @non_resign_count=count(*)
    from TB_S_M_FOREIGN_DEPT_LEVEL A LEFT JOIN
        TB_S_M_FOREIGN_DEPT_LEVEL B ON A.ASSESS_YEAR=B.ASSESS_YEAR and A.ASSESS_TYPE=B.ASSESS_TYPE  and SUBSTRING(B.LEVEL_RATE,1,len(A.LEVEL_RATE))=A.LEVEL_RATE  LEFT JOIN
		TB_S_M_FOREIGN_DIRECTOR_H C ON A.ASSESS_YEAR=C.ASSESS_YEAR and A.ASSESS_TYPE=C.ASSESS_TYPE AND B.LEVEL_RATE=C.LEVEL_RATE
    where A.DEPT_NO= @dept_no_20 and ((B.IS_V_DEPT='N' AND A.DEPT_NO<>B.DEPT_NO)OR(B.IS_V_DEPT='Y' AND A.DEPT_NO=B.DEPT_NO))  and A.HEAD_EMP_ID=@head_emp_id and 
	      B.SIGN_YN<>'Y' and A.ASSESS_YEAR=@ASSESS_YEAR and A.ASSESS_TYPE=@ASSESS_TYPE and A.DEPT_LEVEL>='20' and B.HEAD_EMP_ID<>@HEAD_EMP_ID AND C.DEPT_NO IS NOT NULL
	if(@non_sign_count<=0 and @mail_flag<>'Y' and @non_resign_count<=0 )
	BEGIN

		BEGIN TRY
		    
			select @assess_type_desc=SUB_DESC From TB_9_M_COMM_D  WHERE SYS_CD='SJ' and MAIN_CD='ASSESS_TYPE'  and SUB_CD=@ASSESS_TYPE;

			select @deadline = DEADLINE FROM TB_S_M_FOREIGN_DATA WHERE ASSESS_YEAR=@ASSESS_YEAR and ASSESS_TYPE =@ASSESS_TYPE;

			SELECT @week=DATENAME(DW,@deadline),@month=RIGHT('0'+CONVERT(varchar, DATEPART(MM,@deadline)),2),@day=RIGHT('0'+CONVERT(varchar, DATEPART(dd,@deadline)),2);
			print @deadline;
			print @week;
			--取得部級主管郵件
			SELECT  @company_email=C.COMPANY_EMAIL
			FROM    TB_H_R_DEPT_DATA_AD B  join
				  TB_H_M_EMP C on B.HEAD_EMP_ID = C.EMP_ID
			WHERE  B.DEPT_NO=@dept_no_20
			
			set @MAIL_SUBJECT =
			    N'【重要】'+@ASSESS_YEAR+'年'+@assess_type_desc+'考核部門最終考核實施聯絡';
			
			set @MAIL_CONTENT = 
				N'各部長官 您好<BR>'+
				N'關於本回的'+@assess_type_desc+'考核，您所屬部門各層級主管的初核/複核已經完成，<BR>'+
				N'煩請您進入ACES人事系統-FB2SJ考核作業，完成部門最終考核並於'+@month+'月'+@day+'日（'+@week+'）前提出。<BR>'+
				N'<BR>'+
				N'<BR>'+
				N'以上敬請協力實施，如有任何問題，請與人事擔當聯繫，謝謝!';

				EXEC msdb.dbo.sp_send_dbmail
							@profile_name = 'DB trigger mail',            --這裡輸入Database Mail設定檔的名稱
							@recipients = @company_email,         --要發送的Email
								@body = @MAIL_CONTENT,                   --Email的本內容    
								@body_format = 'HTML',                    --本文的格式,設定為HTML,在Email的本文內可以使用HTML語法
								@subject =@MAIL_SUBJECT ;  
			
			UPDATE TB_S_M_FOREIGN_DEP20_UP_SIGN
			SET MAIL_FLAG='Y'
			WHERE ASSESS_YEAR=@ASSESS_YEAR and ASSESS_TYPE=@ASSESS_TYPE and DEPT_NO=@dept_no_20 and DEPT_LEVEL>='20';

		END TRY
		BEGIN CATCH
		  print 'catch'
			SELECT @pErr = ERROR_NUMBER(), @pErrMsg = '[TB_H_R_SP_LOG:INSERT]'+ERROR_MESSAGE();
			GOTO WriteLog;
		END CATCH


	END;
	
	

WriteLog:
	--新增SP記錄檔
	IF @pErr = 0 BEGIN
		SET @proc_status = 'Y';
		SET @proc_log = '處理成功';
	END
	ELSE BEGIN
		IF @proc_status IS NULL SET @proc_status = 'E';
		SET @proc_log = left(@pErrMsg, 600);
	END
	
	BEGIN TRANSACTION ADD_TB_H_R_SP_LOG
		BEGIN TRY
			INSERT INTO TB_H_R_SP_LOG(PROC_ID, PROC_BASE_DT, PROC_DESC, PROC_OTH_DESC, PROC_DT, PROC_STATUS, PROC_LOG, UPDATED_BY, FUNC_ID)
			VALUES(@proc_id, @ASSESS_YEAR, @proc_desc, NULL, CURRENT_TIMESTAMP, @proc_status, @proc_log, @USERID, @FUNCID);
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION ADD_TB_H_R_SP_LOG;
			SELECT @pErr = ERROR_NUMBER(), @pErrMsg = '[ADD_TB_H_R_SP_LOG:INSERT]:' + ERROR_MESSAGE();
		END CATCH
	COMMIT TRANSACTION ADD_TB_H_R_SP_LOG;

	RETURN (@pErr)	

END






GO


