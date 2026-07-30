USE [FB2DB]
GO

/****** Object:  StoredProcedure [dbo].[SP_S_ASSESS_DEP20_MAIL_CHKDT_MAIL]    Script Date: 2026/7/29 下午 04:31:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER procedure   [dbo].[SP_S_FASSESS_DEP20_MAIL_CHKDT_MAIL]
	
as
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;
	--錯誤訊息
	DECLARE @pRowCount INT =0;
	DECLARE @pErr INT = 0;
	DECLARE @pErrMsg VARCHAR(1000) = NULL;
	
	DECLARE @proc_id_previous VARCHAR(60) = '[SP_S_FASSESS_DEP20_MAIL_CHKDT_MAIL]';
	DECLARE @proc_id VARCHAR(60) = '[SP_S_FASSESS_DEP20_MAIL_CHKDT_MAIL]';
	DECLARE @proc_desc NVARCHAR(120) = '考核表通知部長部門未結通知作業';
	DECLARE @proc_scheduling VARCHAR(30) = 'CALL';
	DECLARE @proc_log NVARCHAR(600) = NULL;
	DECLARE @proc_status VARCHAR(1) = NULL;
	DECLARE @proc_y_cnt INT;

	DECLARE @assess_year varchar(4)='';
	DECLARE @assess_type varchar(1)='';
	DECLARE @mail_chkdt datetime;
	DECLARE @mail_flag varchar(1);
	DECLARE @dept_no_20 varchar(7);
	DECLARE @dept_no varchar(7);
	DECLARE @dept_name varchar(60);
	DECLARE @head_emp_id varchar(5);
	DECLARE @head_emp_name varchar(30);
	DECLARE @company_email varchar(60);
	DECLARE @assess_type_desc varchar(20);
	DECLARE @deadline datetime;
	DECLARE @week varchar(10);
	DECLARE @month varchar(2);
	DECLARE @day varchar(2)
	DECLARE @send_flag varchar(1);
	
	--mail檔的資料
	declare 
	@MAIL_SUBJECT	NVARCHAR(100)='' --主旨
   ,@MAIL_M_CONTENT	NVARCHAR(MAX)=''  --信件內容
   ,@MAIL_CONTENT	NVARCHAR(MAX)=''  --信件內容
   ,@MAILTO			VARCHAR(36)=''--mail通知人
   ,@MAIL_DESC		NVARCHAR(20)=''--mail說明

	-- 取得尚未完成的TB_S_M_FOREIGN_TARGET
	SELECT @assess_year=A.ASSESS_YEAR, @assess_type=A.ASSESS_TYPE, @mail_chkdt=A.MAIL_CHKDT,@deadline=A.DEADLINE,@assess_type_desc=B.SUB_DESC,
	       @send_flag=A.MAIL_DEP20_SEND_FLAG
	FROM TB_S_M_FOREIGN_DATA A JOIN 
	     TB_9_M_COMM_D B  with (nolock)  on B.SYS_CD='FJ' and B.MAIN_CD='ASSESS_TYPE' and B.SUB_CD= A.ASSESS_TYPE and B.IS_VALID='Y' 
	WHERE A.APPROVE_STATUS='N';
	
	if(@assess_year<>'' and  @assess_type<>'' and  @send_flag='N' 
	    and convert(char, @mail_chkdt, 101)= convert(char, getdate(), 101) 
	)
	BEGIN
		BEGIN TRY

			SELECT @week=DATENAME(DW,GETDATE()),@month=RIGHT('0'+CONVERT(varchar, DATEPART(MM,@deadline)),2),@day=RIGHT('0'+CONVERT(varchar, DATEPART(dd,@deadline)),2);

			set @MAIL_SUBJECT =
			    N'【提醒】'+@ASSESS_YEAR+'年'+@assess_type_desc+'考核實施請於'+@month+'月'+@day+'日（'+@week+'）前提出';
			
			set @MAIL_M_CONTENT = 
				N'各部長官 您好<BR>'+
				N'關於本回的'+@assess_type_desc+'考核，提出期限為'+@month+'月'+@day+'日（'+@week+'），<BR>'+
				N'提醒您進入ACES人事系統-FB2SJ考核作業，完成部門最終考核，並於期限內提出。<BR>'+
				N'<BR>'+
				N'<BR>'+
				N'以上敬請協力實施，如有任何問題，請與人事擔當聯繫，謝謝!';

			--依部處取得尚未完成簽核送出的部門
			--取得所有DEPT_NO_20
			DECLARE cur_assess_dept_20 CURSOR FOR
			SELECT A.DEPT_NO,C.EMP_NAME, C.COMPANY_EMAIL
			FROM  TB_S_M_FOREIGN_DEP20_UP_SIGN A join
				  TB_H_R_DEPT_DATA_AD B on A.DEPT_NO=B.DEPT_NO join
				  TB_H_M_EMP C on B.HEAD_EMP_ID = C.EMP_ID
			WHERE A.ASSESS_YEAR=@assess_year and A.ASSESS_TYPE=@assess_type and A.DEPT_LEVEL='20' AND ISNULL(A.SIGN_YN,'')<>'Y'
		
			--開啓CURSOR
			OPEN cur_assess_dept_20;
			FETCH NEXT FROM cur_assess_dept_20
			INTO
				 @dept_no_20, @head_emp_name, @company_email;
			WHILE(@@FETCH_STATUS=0)
			BEGIN
				print 'dept_no_20:'+@dept_no_20;
				DECLARE cur_none_sign_dept CURSOR FOR
				SELECT A.DEPT_NO,A.DEPT_FULL_NAME 
				FROM   TB_S_M_FOREIGN_DIRECTOR_H A
				WHERE A.ASSESS_YEAR=@assess_year and A.ASSESS_TYPE=@assess_type and A.SIGN_YN='N' and
					  A.DEPT_NO IN(
					   SELECT Y.DEPT_NO 
					   FROM 
						(select ASSESS_YEAR, ASSESS_TYPE, LEVEL_RATE from TB_S_M_FOREIGN_DEPT_LEVEL  where DEPT_NO=@dept_no_20 and DEPT_LEVEL>='20' and ASSESS_YEAR=@assess_year and ASSESS_TYPE=@assess_type) X left join  
						TB_S_M_FOREIGN_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
					  )
					  set @MAIL_CONTENT='';
					OPEN cur_none_sign_dept;
					FETCH NEXT FROM cur_none_sign_dept
					INTO
						 @dept_no, @dept_name;

					WHILE(@@FETCH_STATUS=0)
						BEGIN
						   print '--dept_no:'+@dept_no;
							SET @MAIL_CONTENT =@MAIL_CONTENT+ '<tr><td>'+@dept_no+'-'+@dept_name+'</td></tr>';

							FETCH NEXT FROM cur_none_sign_dept
							INTO
								 @dept_no, @dept_name;
						END
					--關閉&釋放cursor
					CLOSE cur_none_sign_dept;
					DEALLOCATE cur_none_sign_dept;
					if @MAIL_CONTENT<>'' 
						BEGIN

						 set @MAIL_CONTENT = @MAIL_M_CONTENT+'<BR>'+ '<table><tr><td >以下為尚未完成簽核的部門</td></tr>'+@MAIL_CONTENT+'</table>';
						 --set @MAIL_SUBJECT = '未完成簽核部門通知';
						 EXEC msdb.dbo.sp_send_dbmail
								@profile_name = 'DB trigger mail',            --這裡輸入Database Mail設定檔的名稱
								@recipients =@company_email,         --要發送的Email
								@body = @MAIL_CONTENT,                   --Email的本內容    
								@body_format = 'HTML',                    --本文的格式,設定為HTML,在Email的本文內可以使用HTML語法
								@subject =@MAIL_SUBJECT ;  
							print @MAIL_CONTENT;
						END

			FETCH NEXT FROM cur_assess_dept_20
			INTO
				@dept_no_20, @head_emp_name, @company_email;
			END;
			--關閉&釋放cursor
			CLOSE cur_assess_dept_20;
			DEALLOCATE cur_assess_dept_20;

			--更新為已發送
			UPDATE TB_S_M_FOREIGN_DATA SET MAIL_DEP20_SEND_FLAG='Y' 
			WHERE APPROVE_STATUS='N' AND ASSESS_YEAR=@assess_year AND ASSESS_TYPE=@assess_type ;
		END TRY
		BEGIN CATCH
			SELECT @pErr = ERROR_NUMBER(), @pErrMsg = '[TB_H_R_SP_LOG:INSERT]'+ERROR_MESSAGE();
			GOTO WriteLog;
		END CATCH

	END

	
	

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
			VALUES(@proc_id, @ASSESS_YEAR, @proc_desc, NULL, CURRENT_TIMESTAMP, @proc_status, @proc_log, '18479', '');
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION ADD_TB_H_R_SP_LOG;
			SELECT @pErr = ERROR_NUMBER(), @pErrMsg = '[ADD_TB_H_R_SP_LOG:INSERT]:' + ERROR_MESSAGE();
		END CATCH
	COMMIT TRANSACTION ADD_TB_H_R_SP_LOG;

	RETURN (@pErr)	

END






GO


