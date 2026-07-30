USE [FB2DB]
GO

/****** Object:  StoredProcedure [dbo].[SP_S_FASSESS_GEN_DIRECTOR]    Script Date: 2026/7/29 下午 02:09:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure   [dbo].[SP_S_FASSESS_GEN_DIRECTOR] 
@ASSESS_YEAR VARCHAR(4),
@ASSESS_TYPE VARCHAR(1),
@DEPT_NO VARCHAR(7), 
@USERID varchar(20),
@FUNCID varchar(30)
AS 
BEGIN
	SET NOCOUNT ON;
	--錯誤訊息
	DECLARE @pRowCount INT =0;
	DECLARE @pErr INT = 0;
	DECLARE @pErrMsg VARCHAR(1000) = NULL;
	
	DECLARE @proc_id_previous VARCHAR(60) = 'SP_S_FASSESS_GEN_DIRECTOR';
	DECLARE @proc_id VARCHAR(60) = 'SP_S_FASSESS_GEN_DIRECTOR';
	DECLARE @proc_desc NVARCHAR(120) = '外籍考課直屬主管資料檔生成';
	DECLARE @proc_scheduling VARCHAR(30) = 'CALL';
	DECLARE @proc_log NVARCHAR(600) = NULL;
	DECLARE @proc_status VARCHAR(1) = NULL;
	DECLARE @proc_y_cnt INT;

	DECLARE @direc_emp_id varchar(5);
	DECLARE @dept_level decimal(2,0);
	DECLARE @dept_full_name nvarchar(150);
	DECLARE @hc_emp_id varchar(5);
	DECLARE @level_rate varchar(20);
	DECLARE @is_v_dept varchar(1);
--DECLARE @dept_no nvarchar(7);
 
if(select CURSOR_STATUS('global','cur_DIRECTOR'))>-3
begin
		if(select CURSOR_STATUS('global','cur_DIRECTOR'))>=0 CLOSE cur_DIRECTOR;
		DEALLOCATE cur_DIRECTOR;
end
if(select CURSOR_STATUS('global','cur_HC'))>-3
begin
		if(select CURSOR_STATUS('global','cur_HC'))>=0 CLOSE cur_HC;
		DEALLOCATE cur_HC;
end
BEGIN TRANSACTION;
BEGIN TRY
--刪除己產出資料
	Delete from TB_S_M_FOREIGN_DIRECTOR_H where ASSESS_YEAR=@assess_year and ASSESS_TYPE=@ASSESS_TYPE;
	Delete from TB_S_M_FOREIGN_DIRECTOR_D where ASSESS_YEAR=@assess_year and ASSESS_TYPE=@ASSESS_TYPE;
print 'C1 開始時間:'+CONVERT(VARCHAR(30),GETDATE(),121);
DECLARE cur_DIRECTOR CURSOR FOR
		select A.DEPT_NO,A.DEPT_LEVEL, B.DEPT_NAME, A.HEAD_EMP_ID, A.LEVEL_RATE, A.IS_V_DEPT
		from TB_S_M_FOREIGN_DEPT_LEVEL A join 
			 TB_H_R_DEPT_DATA B on A.DEPT_NO=B.DEPT_NO
		where ASSESS_YEAR=@ASSESS_YEAR and ASSESS_TYPE = @ASSESS_TYPE and cast(A.DEPT_LEVEL as int)>=20 order by A.LEVEL_RATE ;
--開啓CURSOR
    OPEN cur_DIRECTOR;
	FETCH NEXT FROM cur_DIRECTOR
	INTO
	     @dept_no, @dept_level, @dept_full_name, @direc_emp_id, @level_rate, @is_v_dept;
WHILE(@@FETCH_STATUS=0)
		BEGIN
		IF(RIGHT(@dept_no,5)='00000' AND @dept_level='30' AND @is_v_dept='Y')
		BEGIN
			PRINT 'empty';
		END
		ELSE
		BEGIN
		INSERT INTO TB_S_M_FOREIGN_DIRECTOR_H(ASSESS_YEAR, ASSESS_TYPE, DIREC_EMP_ID, MNG_NUM, SIGN_YN, 
		                                     DEPT_NO, DEPT_FULL_NAME, DEPT_LEVEL, LEVEL_RATE,
											 CREATED_BY, CREATED_DT,UPDATED_BY, UPDATED_DT,FUNC_ID)
		SELECT @ASSESS_YEAR,@ASSESS_TYPE, @direc_emp_id, count(*) ,'N',
			   @dept_no, @dept_full_name, @dept_level,@level_rate,
			   @USERID,getDate(),@USERID,getDate(), @FUNCID
		from(
			select A.EMP_ID ,A.EMP_NAME from TB_S_M_FOREIGN_TARGET A WHERE A.ASSESS_YEAR=@ASSESS_YEAR and A.ASSESS_TYPE=@ASSESS_TYPE 
												and A.DEPT_NO=@dept_no and A.EMP_ID<>@direc_emp_id  and Left(A.LEVEL_CD,1)>='3'
			union
			select A.EMP_ID ,A.EMP_NAME from TB_H_R_DEPT_DATA_AD B join 
											 TB_S_M_FOREIGN_TARGET A on B.HEAD_EMP_ID =A.EMP_ID JOIN
		                                     TB_H_R_DEPT_DATA_AD C ON A.DEPT_NO=C.DEPT_NO 
										WHERE A.ASSESS_YEAR=@ASSESS_YEAR and A.ASSESS_TYPE=@ASSESS_TYPE and B.UP_DEPT_NO=@dept_no and A.EMP_ID<>@direc_emp_id  and Left(A.LEVEL_CD,1)>='3' AND
										 C.UP_DEPT_NO=@dept_no
										)T ;

		INSERT INTO TB_S_M_FOREIGN_DIRECTOR_D(ASSESS_YEAR, ASSESS_TYPE, DIREC_EMP_ID, DEPT_NO, EMP_ID,LEVEL_RATE,
											 CREATED_BY, CREATED_DT,UPDATED_BY, UPDATED_DT,FUNC_ID)
		SELECT @ASSESS_YEAR,@ASSESS_TYPE, @direc_emp_id, @dept_no, T.EMP_ID,@level_rate,
			   @USERID,getDate(),@USERID,getDate(), @FUNCID
		from(
			select A.EMP_ID ,A.EMP_NAME,A.PJOB_CD from TB_S_M_FOREIGN_TARGET A WHERE A.ASSESS_YEAR=@ASSESS_YEAR and A.ASSESS_TYPE=@ASSESS_TYPE and A.DEPT_NO=@dept_no and A.EMP_ID<>@direc_emp_id and Left(A.LEVEL_CD,1)>='3'
			union
			select A.EMP_ID ,A.EMP_NAME,A.PJOB_CD from TB_H_R_DEPT_DATA_AD B join 
											 TB_S_M_FOREIGN_TARGET A on B.HEAD_EMP_ID =A.EMP_ID JOIN
		                                     TB_H_R_DEPT_DATA_AD C ON A.DEPT_NO=C.DEPT_NO 
										WHERE A.ASSESS_YEAR=@ASSESS_YEAR and A.ASSESS_TYPE=@ASSESS_TYPE and B.UP_DEPT_NO=@dept_no and A.EMP_ID<>@direc_emp_id and Left(A.LEVEL_CD,1)>='3'  AND
										 C.UP_DEPT_NO=@dept_no
										)T 
		--WHERE T.PJOB_CD not IN(select pjob_cd from TB_H_M_PJOB where pjob_desc like '課長' or pjob_desc like 'G長');
		END
		

		FETCH NEXT FROM cur_DIRECTOR
		INTO
	     @dept_no, @dept_level, @dept_full_name, @direc_emp_id, @level_rate, @is_v_dept;
		END;
		CLOSE cur_DIRECTOR;
		DEALLOCATE cur_DIRECTOR;
print 'C2 開始時間:'+CONVERT(VARCHAR(30),GETDATE(),121);
		--處理課長的直屬主管
	DECLARE cur_HC CURSOR FOR
	SELECT distinct T.EMP_ID, C.DIRECT_HEAD_EMP_ID,D.DEPT_NO,E.DEPT_LEVEL,F.DEPT_NAME,E.LEVEL_RATE FROM
		(
			select A.ASSESS_YEAR,A.ASSESS_TYPE,B.DEPT_NO,B.DEPT_NAME,B.HEAD_EMP_ID,A.EMP_ID ,A.EMP_NAME,A.PJOB_CD 
			from TB_S_M_FORIGN_TARGET A LEFT JOIN
			     TB_H_R_DEPT_DATA B ON A.DEPT_NO=B.DEPT_NO WHERE A.ASSESS_YEAR=@ASSESS_YEAR and A.ASSESS_TYPE=@ASSESS_TYPE and A.EMP_ID<>B.HEAD_EMP_ID and Left(A.LEVEL_CD,1)>='3'
			union
				select A.ASSESS_YEAR,A.ASSESS_TYPE, C.DEPT_NO,C.DEPT_NAME, C.HEAD_EMP_ID,A.EMP_ID ,A.EMP_NAME,A.PJOB_CD from TB_S_M_FOREIGN_TARGET A join 
											 TB_H_R_DEPT_DATA_AD B on A.DEPT_NO =B.DEPT_NO JOIN
		                                     TB_H_R_DEPT_DATA_AD C ON B.UP_DEPT_NO=C.DEPT_NO 
										WHERE A.ASSESS_YEAR=@ASSESS_YEAR and A.ASSESS_TYPE=@ASSESS_TYPE and A.EMP_ID=B.HEAD_EMP_ID and Left(A.LEVEL_CD,1)>='3' 
										)T LEFT JOIN		
			TB_H_M_EMP C ON T.EMP_ID=C.EMP_ID LEFT JOIN
			TB_H_M_EMP D ON C.DIRECT_HEAD_EMP_ID= D.EMP_ID LEFT JOIN
			TB_S_M_FOREIGN_DEPT_LEVEL E ON D.DEPT_NO=E.DEPT_NO AND T.ASSESS_YEAR=E.ASSESS_YEAR and T.ASSESS_TYPE = E.ASSESS_TYPE  and C.DIRECT_HEAD_EMP_ID=E.HEAD_EMP_ID LEFT JOIN
			TB_H_R_DEPT_DATA F ON D.DEPT_NO=F.DEPT_NO
		    WHERE T.PJOB_CD in('ME10','ME20','MD10','MD11','MD15','MD20','MD21','MD40','MD50','MD55','PF30','PG30')  and T.HEAD_EMP_ID<>C.DIRECT_HEAD_EMP_ID 
		      AND D.PJOB_CD in('MC20','MC40','MC60','MC30','MB20') and E.DEPT_LEVEL>=20  and RIGHT(D.DEPT_NO,5)='00000' AND E.DEPT_LEVEL='30' AND E.IS_V_DEPT='Y'
		ORDER BY C.DIRECT_HEAD_EMP_ID ,D.DEPT_NO;
	--開啓CURSOR
		OPEN cur_HC;
		FETCH NEXT FROM cur_HC
		INTO
			 @hc_emp_id, @direc_emp_id, @dept_no, @dept_level, @dept_full_name, @level_rate;
	WHILE(@@FETCH_STATUS=0)
			BEGIN
			--更新TB_S_M_FOREIGN_DIRECTOR_H
			UPDATE T1
			SET T1.MNG_NUM=T1.MNG_NUM-1
			FROM TB_S_M_FOREIGN_DIRECTOR_H T1 LEFT JOIN
				 TB_S_M_FOREIGN_DIRECTOR_D T2 ON T1.ASSESS_YEAR=T2.ASSESS_YEAR AND T1.ASSESS_TYPE=T2.ASSESS_TYPE AND 
				                                T1.DEPT_NO=T2.DEPT_NO AND T1.DIREC_EMP_ID=T2.DIREC_EMP_ID
			WHERE T2.EMP_ID=@hc_emp_id and T1.ASSESS_YEAR=@ASSESS_YEAR AND T1.ASSESS_TYPE=@ASSESS_TYPE AND T1.MNG_NUM>0;
			--刪除原有資料
			DELETE FROM TB_S_M_FOREIGN_DIRECTOR_D WHERE EMP_ID=@hc_emp_id;
			
			--檢查部門是否已存在TB_S_M_FOREIGN_DIRECTOR_H
			if(SELECT count(*) FROM TB_S_M_FOREIGN_DIRECTOR_H where DEPT_NO=@dept_no AND DIREC_EMP_ID=@direc_emp_id AND ASSESS_YEAR=@ASSESS_YEAR AND ASSESS_TYPE=@ASSESS_TYPE)=0
			BEGIN
				--新增TB_S_M_FOREIGN_DIRECTOR_H
				INSERT INTO TB_S_M_FOREIGN_DIRECTOR_H(ASSESS_YEAR, ASSESS_TYPE, DIREC_EMP_ID, MNG_NUM, SIGN_YN, 
		                                     DEPT_NO, DEPT_FULL_NAME, DEPT_LEVEL,LEVEL_RATE,
											 CREATED_BY, CREATED_DT,UPDATED_BY, UPDATED_DT,FUNC_ID)values
											 (@ASSESS_YEAR, @ASSESS_TYPE, @direc_emp_id,1,'N',
											  @dept_no, @dept_full_name,@dept_level,@level_rate,
											  @USERID,getDate(),@USERID,getDate(), @FUNCID);
			END
			ELSE
			BEGIN
			--更新TB_S_M_FOREIGN_DIRECTOR_H
				UPDATE TB_S_M_FOREIGN_DIRECTOR_H SET MNG_NUM=MNG_NUM+1
				WHERE ASSESS_YEAR=@ASSESS_YEAR and ASSESS_TYPE = @ASSESS_TYPE AND
				      DEPT_NO=@dept_no AND DIREC_EMP_ID=@direc_emp_id;
			END
			--新增TB_S_M_FOREIGN_DIRECTOR_D
				INSERT INTO TB_S_M_FOREIGN_DIRECTOR_D(ASSESS_YEAR, ASSESS_TYPE, DIREC_EMP_ID, DEPT_NO, EMP_ID,LEVEL_RATE,
											 CREATED_BY, CREATED_DT,UPDATED_BY, UPDATED_DT,FUNC_ID)values
											 (@ASSESS_YEAR, @ASSESS_TYPE, @direc_emp_id,@dept_no,@hc_emp_id, @level_rate,
											   @USERID,getDate(),@USERID,getDate(), @FUNCID);	
			FETCH NEXT FROM cur_HC
			INTO
			 @hc_emp_id, @direc_emp_id, @dept_no, @dept_level, @dept_full_name, @level_rate;
			END;
			CLOSE cur_HC;
			DEALLOCATE cur_HC;
	
		--刪除沒員工的TB_S_M_ASSESS_DIREC_H
		DELETE FROM TB_S_M_FOREIGN_DIRECTOR_H where ASSESS_YEAR=@ASSESS_YEAR and ASSESS_TYPE=@ASSESS_TYPE and MNG_NUM=0;
		COMMIT TRANSACTION;
print 'C2 結束時間:'+CONVERT(VARCHAR(30),GETDATE(),121);
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION ;
		SELECT @pErr = ERROR_NUMBER(), @pErrMsg = '[TB_H_R_SP_LOG:INSERT]'+ERROR_MESSAGE();
		GOTO WriteLog;

	END CATCH
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
END;






GO


