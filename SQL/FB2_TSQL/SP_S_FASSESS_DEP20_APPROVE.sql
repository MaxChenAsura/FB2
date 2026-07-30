USE [FB2DB]
GO
/****** Object:  StoredProcedure [dbo].[SP_S_ASSESS_DEP20_APPROVE]    Script Date: 2026/7/29 下午 04:38:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Max 
-- Create date: 2021/10/26
-- Update date: 2021/10/26
-- Description:	部長考核維護- 考核年度、考核類別,員工ID, 部長部門, 考評, 總評,  使用者id,FunctionID
-- =============================================

ALTER procedure [dbo].[SP_S_FASSESS_DEP20_APPROVE]
	@ASSESS_YEAR varchar(4), 
	@ASSESS_TYPE varchar(1), 
	@EMP_ID varchar(5), 
	@DEPT_NO_20 varchar(7), 
	@SCORE_DEPT varchar(2),
	@RECOMM_DESC varchar(80),
	@COMMENTS nvarchar(500),
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
	
	DECLARE @proc_id_previous VARCHAR(60) = 'SP_S_FASSESS_DEP20_APPROVE';
	DECLARE @proc_id VARCHAR(60) = 'SP_S_FASSESS_DEP20_APPROVE';
	DECLARE @proc_desc NVARCHAR(120) = '部長考核維護';
	DECLARE @proc_scheduling VARCHAR(30) = 'CALL';
	DECLARE @proc_log NVARCHAR(600) = NULL;
	DECLARE @proc_status VARCHAR(1) = NULL;
	DECLARE @proc_y_cnt INT;

	
	DECLARE @ori_score_final varchar(2);
	DECLARE @is_out varchar(1);
	DECLARE @ori_comments  nvarchar(500);
	DECLARE @new_comments  nvarchar(500);
	DECLARE @level_cd varchar(3);
	DECLARE @ws_cd varchar(1);
	DECLARE @ma_type varchar(1);
	DECLARE @ma_emp_id varchar(5);
	DECLARE @strSQL nvarchar(4000);  --一定要nvarchar
	DECLARE @strSQL1 nvarchar(2000);  --一定要nvarchar,
	DECLARE @score_level_group_exit int;  

	--取得考核員工TB_S_M_FOREIGN_TARGET資料
	SELECT  @ori_score_final=SCORE_FINAL, @is_out=isnull(IS_OUT,'N'), @ori_comments=isnull(COMMENTS,''), @level_cd=level_cd,@ws_cd=WS_CD
	FROM   TB_S_M_FOREIGN_TARGET 
	WHERE  ASSESS_YEAR=@ASSESS_YEAR and ASSESS_TYPE = @ASSESS_TYPE and EMP_ID=@EMP_ID;
	print 'level_cd:'+@level_cd;
	print 'ws_cd:'+@ws_cd;
	print 'dept_no_20:'+@DEPT_NO_20;
	--取得score_level_group
	select @score_level_group_exit=count(*) from TB_S_M_FOREIGN_DEP20_PEO where DEPT_NO_20=@DEPT_NO_20 and WS_CD=@ws_cd and SCORE_LEVEL_GROUP<>'';
	print 'is_out:'+@is_out;
	DECLARE cur_MA CURSOR FOR
	SELECT EMP_ID MA_EMP_ID, case when DEPT_LEVEL='10' then 'A' else 'B' end MA_TYPE from TB_S_M_FOREIGN_DEP20_UP_SIGN  
	where  DEPT_NO=@DEPT_NO_20 and ASSESS_YEAR=@ASSESS_YEAR and ASSESS_TYPE = @ASSESS_TYPE and DEPT_LEVEL<'20';

	BEGIN TRANSACTION
	BEGIN TRY
		--新增TB_S_M_FOREIGN_LOG
		insert into TB_S_M_FOREIGN_LOG (ASSESS_YEAR, ASSESS_TYPE, EMP_ID, GRADE, MEMO,CREATED_BY,CREATED_DT,FUNC_ID)
		values(@ASSESS_YEAR, @ASSESS_TYPE, @EMP_ID, @SCORE_DEPT, @COMMENTS,  @USERID,GETDATE(),@FUNCID);
		--更新TB_S_M_FOREIGN_DEP20_PEO
		/**
		if @is_out='N'
				 BEGIN
					

					set @strSQL='UPDATE TB_S_M_FOREIGN_DEP20_PEO SET REAL_'+@ori_score_final+' = REAL_'+@ori_score_final+'-1 
					               WHERE ASSESS_TYPE='''+@ASSESS_TYPE+''' and ASSESS_YEAR='''+@ASSESS_YEAR+''' and DEPT_NO_20='''+@DEPT_NO_20+'''  and 
											   IS_MERGER<>''A'' and WS_CD='''+@ws_cd+'''';   
					if @score_level_group_exit>0 set @strSQL=@strSQL+ ' and CHARINDEX(SCORE_LEVEL_GROUP,'''+@level_cd+''')>0';
											   print @strSQL;
					exec sp_executesql @strSQL;   

					set @strSQL='UPDATE TB_S_M_FOREIGN_DEP20_PEO SET REAL_'+@SCORE_DEPT+' = REAL_'+@SCORE_DEPT+'+1 
					               WHERE ASSESS_TYPE='''+@ASSESS_TYPE+''' and ASSESS_YEAR='''+@ASSESS_YEAR+''' and DEPT_NO_20='''+@DEPT_NO_20+'''  and 
											   IS_MERGER<>''A'' and WS_CD='''+@ws_cd+''''; 
					if @score_level_group_exit>0 set @strSQL=@strSQL+ ' and CHARINDEX(SCORE_LEVEL_GROUP,'''+@level_cd+''')>0';
											     print @strSQL;  
					exec sp_executesql @strSQL;  

				 END
				ELSE BEGIN
					

					set @strSQL='UPDATE TB_S_M_FOREIGN_DEP20_PEO SET OUT_REAL_'+@ori_score_final+' = OUT_REAL_'+@ori_score_final+'-1 
					               WHERE ASSESS_TYPE='''+@ASSESS_TYPE+''' and ASSESS_YEAR='''+@ASSESS_YEAR+''' and DEPT_NO_20='''+@DEPT_NO_20+'''  and 
											   IS_MERGER<>''A''  and WS_CD='''+@ws_cd+'''';  
					if @score_level_group_exit>0 set @strSQL=@strSQL+ ' and CHARINDEX(SCORE_LEVEL_GROUP,'''+@level_cd+''')>0'; 
					exec sp_executesql @strSQL;   

					set @strSQL='UPDATE TB_S_M_FOREIGN_DEP20_PEO SET OUT_REAL_'+@SCORE_DEPT+' = OUT_REAL_'+@SCORE_DEPT+'+1 
					               WHERE ASSESS_TYPE='''+@ASSESS_TYPE+''' and ASSESS_YEAR='''+@ASSESS_YEAR+''' and DEPT_NO_20='''+@DEPT_NO_20+'''  and 
											   IS_MERGER<>''A''  and WS_CD='''+@ws_cd+''''; 
					if @score_level_group_exit>0 set @strSQL=@strSQL+ ' and CHARINDEX(SCORE_LEVEL_GROUP,'''+@level_cd+''')>0';  
					exec sp_executesql @strSQL;   

				END;
            **/
		  --開啓CURSOR
		OPEN cur_MA;
		FETCH NEXT FROM cur_MA
		INTO
	       @ma_emp_id, @ma_type;
		   WHILE(@@FETCH_STATUS=0)
			BEGIN
				--更新TB_S_M_FOREIGN_MA_PEO
				if @is_out='N'
				 BEGIN
				     print 'not is_out';
					set @strSQL='UPDATE TB_S_M_FOREIGN_MA_PEO SET REAL_'+@ori_score_final+' = REAL_'+@ori_score_final+'-1 
					             WHERE ASSESS_TYPE='''+@ASSESS_TYPE+''' and ASSESS_YEAR='''+@ASSESS_YEAR+''' and MA_TYPE='''+@MA_TYPE+''' and MA_EMP_ID='''+@ma_emp_id+''' and 
											   GRP_CD=(SELECT H.GRP_CD FROM TB_S_M_FOREIGN_GROUP_H H join 
																		  TB_S_M_FOREIGN_GROUP_D D on H.ASSESS_TYPE=D.ASSESS_TYPE and H.ASSESS_YEAR =D.ASSESS_YEAR and H.GRP_CD=D.GRP_CD  
													   WHERE D.LEVEL_CD='''+@level_cd+''' and H.ASSESS_TYPE='''+@ASSESS_TYPE+''' and H.ASSESS_YEAR='''+@ASSESS_YEAR+''' and H.WS_CD='''+@ws_cd+''' );'
					--print @strSQL;
					exec sp_executesql @strSQL;
					
					set @strSQL='UPDATE TB_S_M_FOREIGN_MA_PEO SET REAL_'+@SCORE_DEPT+' = REAL_'+@SCORE_DEPT+'+1 
					             WHERE ASSESS_TYPE='''+@ASSESS_TYPE+''' and ASSESS_YEAR='''+@ASSESS_YEAR+''' and MA_TYPE='''+@MA_TYPE+''' and MA_EMP_ID='''+@ma_emp_id+''' and 
											   GRP_CD=(SELECT H.GRP_CD FROM TB_S_M_FOREIGN_GROUP_H H join 
																		  TB_S_M_FOREIGN_GROUP_D D on H.ASSESS_TYPE=D.ASSESS_TYPE and H.ASSESS_YEAR =D.ASSESS_YEAR and H.GRP_CD=D.GRP_CD  
													   WHERE D.LEVEL_CD='''+@level_cd+''' and H.ASSESS_TYPE='''+@ASSESS_TYPE+''' and H.ASSESS_YEAR='''+@ASSESS_YEAR+''' and H.WS_CD='''+@ws_cd+''' );'
													     --print @strSQL;
					exec sp_executesql @strSQL;          
					/**
					if @ori_score_final = 'A'  
						BEGIN
						UPDATE TB_S_M_FOREIGN_MA_PEO SET REAL_A=REAL_A-1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and MA_TYPE=@MA_TYPE and MA_EMP_ID=@ma_emp_id and 
											   GRP_CD=(SELECT H.GRP_CD FROM TB_S_M_FOREIGN_GROUP_H H join 
																		  TB_S_M_FOREIGN_GROUP_D D on H.ASSESS_TYPE=D.ASSESS_TYPE and H.ASSESS_YEAR =D.ASSESS_YEAR and H.GRP_CD=D.GRP_CD  
													   WHERE D.LEVEL_CD=@level_cd and H.ASSESS_TYPE=@ASSESS_TYPE and H.ASSESS_YEAR=@ASSESS_YEAR and H.WS_CD=@ws_cd );

						UPDATE TB_S_M_FOREIGN_DEP20_PEO SET REAL_A=REAL_A-1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and DEPT_NO_20=@DEPT_NO_20   and 
											   IS_MERGER<>'A' and CHARINDEX(SCORE_LEVEL_GROUP,@level_cd)>0
						END;
					if @ori_score_final = 'B'  
						UPDATE TB_S_M_FOREIGN_MA_PEO SET REAL_B=REAL_B-1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and MA_TYPE=@MA_TYPE and MA_EMP_ID=@ma_emp_id  and 
											   GRP_CD=(SELECT H.GRP_CD FROM TB_S_M_FOREIGN_GROUP_H H join 
																		  TB_S_M_FOREIGN_GROUP_D D on H.ASSESS_TYPE=D.ASSESS_TYPE and H.ASSESS_YEAR =D.ASSESS_YEAR and H.GRP_CD=D.GRP_CD  
													   WHERE D.LEVEL_CD=@level_cd and H.ASSESS_TYPE=@ASSESS_TYPE and H.ASSESS_YEAR=@ASSESS_YEAR and H.WS_CD=@ws_cd );
					if @ori_score_final = 'C'  UPDATE TB_S_M_FOREIGN_MA_PEO SET REAL_C=REAL_C-1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and MA_TYPE=@MA_TYPE and MA_EMP_ID=@ma_emp_id  and 
											   GRP_CD=(SELECT H.GRP_CD FROM TB_S_M_FOREIGN_GROUP_H H join 
																		  TB_S_M_FOREIGN_GROUP_D D on H.ASSESS_TYPE=D.ASSESS_TYPE and H.ASSESS_YEAR =D.ASSESS_YEAR and H.GRP_CD=D.GRP_CD  
													   WHERE D.LEVEL_CD=@level_cd and H.ASSESS_TYPE=@ASSESS_TYPE and H.ASSESS_YEAR=@ASSESS_YEAR and H.WS_CD=@ws_cd );
					if @ori_score_final = 'D'  UPDATE TB_S_M_FOREIGN_MA_PEO SET REAL_D=REAL_D-1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and MA_TYPE=@MA_TYPE and MA_EMP_ID=@ma_emp_id  and 
											   GRP_CD=(SELECT H.GRP_CD FROM TB_S_M_FOREIGN_GROUP_H H join 
																		  TB_S_M_FOREIGN_GROUP_D D on H.ASSESS_TYPE=D.ASSESS_TYPE and H.ASSESS_YEAR =D.ASSESS_YEAR and H.GRP_CD=D.GRP_CD  
													   WHERE D.LEVEL_CD=@level_cd and H.ASSESS_TYPE=@ASSESS_TYPE and H.ASSESS_YEAR=@ASSESS_YEAR and H.WS_CD=@ws_cd );
					if @ori_score_final = 'E'  UPDATE TB_S_M_FOREIGN_MA_PEO SET REAL_E=REAL_E-1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and MA_TYPE=@MA_TYPE and MA_EMP_ID=@ma_emp_id  and 
											   GRP_CD=(SELECT H.GRP_CD FROM TB_S_M_FOREIGN_GROUP_H H join 
																		  TB_S_M_FOREIGN_GROUP_D D on H.ASSESS_TYPE=D.ASSESS_TYPE and H.ASSESS_YEAR =D.ASSESS_YEAR and H.GRP_CD=D.GRP_CD  
													   WHERE D.LEVEL_CD=@level_cd and H.ASSESS_TYPE=@ASSESS_TYPE and H.ASSESS_YEAR=@ASSESS_YEAR and H.WS_CD=@ws_cd );
					if @SCORE_DEPT = 'A'  UPDATE TB_S_M_FOREIGN_MA_PEO SET REAL_A=REAL_A+1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and MA_TYPE=@MA_TYPE and MA_EMP_ID=@ma_emp_id  and 
											   GRP_CD=(SELECT H.GRP_CD FROM TB_S_M_FOREIGN_GROUP_H H join 
																		  TB_S_M_FOREIGN_GROUP_D D on H.ASSESS_TYPE=D.ASSESS_TYPE and H.ASSESS_YEAR =D.ASSESS_YEAR and H.GRP_CD=D.GRP_CD  
													   WHERE D.LEVEL_CD=@level_cd and H.ASSESS_TYPE=@ASSESS_TYPE and H.ASSESS_YEAR=@ASSESS_YEAR and H.WS_CD=@ws_cd );
					if @SCORE_DEPT = 'B'  UPDATE TB_S_M_FOREIGN_MA_PEO SET REAL_B=REAL_B+1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and MA_TYPE=@MA_TYPE and MA_EMP_ID=@ma_emp_id  and 
											   GRP_CD=(SELECT H.GRP_CD FROM TB_S_M_FOREIGN_GROUP_H H join 
																		  TB_S_M_FOREIGN_GROUP_D D on H.ASSESS_TYPE=D.ASSESS_TYPE and H.ASSESS_YEAR =D.ASSESS_YEAR and H.GRP_CD=D.GRP_CD  
													   WHERE D.LEVEL_CD=@level_cd and H.ASSESS_TYPE=@ASSESS_TYPE and H.ASSESS_YEAR=@ASSESS_YEAR and H.WS_CD=@ws_cd );
					if @SCORE_DEPT = 'C'  UPDATE TB_S_M_FOREIGN_MA_PEO SET REAL_C=REAL_C+1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and MA_TYPE=@MA_TYPE and MA_EMP_ID=@ma_emp_id  and 
											   GRP_CD=(SELECT H.GRP_CD FROM TB_S_M_FOREIGN_GROUP_H H join 
																		  TB_S_M_FOREIGN_GROUP_D D on H.ASSESS_TYPE=D.ASSESS_TYPE and H.ASSESS_YEAR =D.ASSESS_YEAR and H.GRP_CD=D.GRP_CD  
													   WHERE D.LEVEL_CD=@level_cd and H.ASSESS_TYPE=@ASSESS_TYPE and H.ASSESS_YEAR=@ASSESS_YEAR and H.WS_CD=@ws_cd );
					if @SCORE_DEPT = 'D'  UPDATE TB_S_M_FOREIGN_MA_PEO SET REAL_D=REAL_D+1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and MA_TYPE=@MA_TYPE and MA_EMP_ID=@ma_emp_id  and 
											   GRP_CD=(SELECT H.GRP_CD FROM TB_S_M_FOREIGN_GROUP_H H join 
																		  TB_S_M_FOREIGN_GROUP_D D on H.ASSESS_TYPE=D.ASSESS_TYPE and H.ASSESS_YEAR =D.ASSESS_YEAR and H.GRP_CD=D.GRP_CD  
													   WHERE D.LEVEL_CD=@level_cd and H.ASSESS_TYPE=@ASSESS_TYPE and H.ASSESS_YEAR=@ASSESS_YEAR and H.WS_CD=@ws_cd );
					if @SCORE_DEPT = 'E'  UPDATE TB_S_M_FOREIGN_MA_PEO SET REAL_E=REAL_E+1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and MA_TYPE=@MA_TYPE and MA_EMP_ID=@ma_emp_id  and 
											   GRP_CD=(SELECT H.GRP_CD FROM TB_S_M_FOREIGN_GROUP_H H join 
																		  TB_S_M_FOREIGN_GROUP_D D on H.ASSESS_TYPE=D.ASSESS_TYPE and H.ASSESS_YEAR =D.ASSESS_YEAR and H.GRP_CD=D.GRP_CD  
													   WHERE D.LEVEL_CD=@level_cd and H.ASSESS_TYPE=@ASSESS_TYPE and H.ASSESS_YEAR=@ASSESS_YEAR and H.WS_CD=@ws_cd );
					**/
				END
				ELSE BEGIN
				 print 'is_out';
				set @strSQL='UPDATE TB_S_M_FOREIGN_MA_PEO SET OUT_REAL_'+@ori_score_final+' = OUT_REAL_'+@ori_score_final+'-1 
					             WHERE ASSESS_TYPE='''+@ASSESS_TYPE+''' and ASSESS_YEAR='''+@ASSESS_YEAR+''' and MA_TYPE='''+@MA_TYPE+''' and MA_EMP_ID='''+@ma_emp_id+''' and 
											   GRP_CD=(SELECT H.GRP_CD FROM TB_S_M_FOREIGN_GROUP_H H join 
																		  TB_S_M_FOREIGN_GROUP_D D on H.ASSESS_TYPE=D.ASSESS_TYPE and H.ASSESS_YEAR =D.ASSESS_YEAR and H.GRP_CD=D.GRP_CD  
													   WHERE D.LEVEL_CD='''+@level_cd+''' and H.ASSESS_TYPE='''+@ASSESS_TYPE+''' and H.ASSESS_YEAR='''+@ASSESS_YEAR+''' and H.WS_CD='''+@ws_cd+''' );'
					exec sp_executesql @strSQL;
					
					set @strSQL='UPDATE TB_S_M_FOREIGN_MA_PEO SET OUT_REAL_'+@SCORE_DEPT+' = OUT_REAL_'+@SCORE_DEPT+'+1 
					             WHERE ASSESS_TYPE='''+@ASSESS_TYPE+''' and ASSESS_YEAR='''+@ASSESS_YEAR+''' and MA_TYPE='''+@MA_TYPE+''' and MA_EMP_ID='''+@ma_emp_id+''' and 
											   GRP_CD=(SELECT H.GRP_CD FROM TB_S_M_FOREIGN_GROUP_H H join 
																		  TB_S_M_FOREIGN_GROUP_D D on H.ASSESS_TYPE=D.ASSESS_TYPE and H.ASSESS_YEAR =D.ASSESS_YEAR and H.GRP_CD=D.GRP_CD  
													   WHERE D.LEVEL_CD='''+@level_cd+''' and H.ASSESS_TYPE='''+@ASSESS_TYPE+''' and H.ASSESS_YEAR='''+@ASSESS_YEAR+''' and H.WS_CD='''+@ws_cd+''' );'
					exec sp_executesql @strSQL;         
				/**
					if @ori_score_final = 'A'  
						BEGIN
						UPDATE TB_S_M_FOREIGN_MA_PEO SET OUT_REAL_A=OUT_REAL_A-1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and MA_TYPE=@MA_TYPE and MA_EMP_ID=@ma_emp_id  and 
											   GRP_CD=(SELECT H.GRP_CD FROM TB_S_M_FOREIGN_GROUP_H H join 
																		  TB_S_M_FOREIGN_GROUP_D D on H.ASSESS_TYPE=D.ASSESS_TYPE and H.ASSESS_YEAR =D.ASSESS_YEAR and H.GRP_CD=D.GRP_CD  
													   WHERE D.LEVEL_CD=@level_cd and H.ASSESS_TYPE=@ASSESS_TYPE and H.ASSESS_YEAR=@ASSESS_YEAR and H.WS_CD=@ws_cd );
					    END;
					if @ori_score_final = 'B'  UPDATE TB_S_M_FOREIGN_MA_PEO SET OUT_REAL_B=OUT_REAL_B-1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and MA_TYPE=@MA_TYPE and MA_EMP_ID=@ma_emp_id  and 
											   GRP_CD=(SELECT H.GRP_CD FROM TB_S_M_FOREIGN_GROUP_H H join 
																		  TB_S_M_FOREIGN_GROUP_D D on H.ASSESS_TYPE=D.ASSESS_TYPE and H.ASSESS_YEAR =D.ASSESS_YEAR and H.GRP_CD=D.GRP_CD  
													   WHERE D.LEVEL_CD=@level_cd and H.ASSESS_TYPE=@ASSESS_TYPE and H.ASSESS_YEAR=@ASSESS_YEAR and H.WS_CD=@ws_cd );
					if @ori_score_final = 'C'  UPDATE TB_S_M_FOREIGN_MA_PEO SET OUT_REAL_C=OUT_REAL_C-1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and MA_TYPE=@MA_TYPE and MA_EMP_ID=@ma_emp_id  and 
											   GRP_CD=(SELECT H.GRP_CD FROM TB_S_M_FOREIGN_GROUP_H H join 
																		  TB_S_M_FOREIGN_GROUP_D D on H.ASSESS_TYPE=D.ASSESS_TYPE and H.ASSESS_YEAR =D.ASSESS_YEAR and H.GRP_CD=D.GRP_CD  
													   WHERE D.LEVEL_CD=@level_cd and H.ASSESS_TYPE=@ASSESS_TYPE and H.ASSESS_YEAR=@ASSESS_YEAR and H.WS_CD=@ws_cd );
					if @ori_score_final = 'D'  UPDATE TB_S_M_FOREIGN_MA_PEO SET OUT_REAL_D=OUT_REAL_D-1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and MA_TYPE=@MA_TYPE and MA_EMP_ID=@ma_emp_id  and 
											   GRP_CD=(SELECT H.GRP_CD FROM TB_S_M_FOREIGN_GROUP_H H join 
																		  TB_S_M_FOREIGN_GROUP_D D on H.ASSESS_TYPE=D.ASSESS_TYPE and H.ASSESS_YEAR =D.ASSESS_YEAR and H.GRP_CD=D.GRP_CD  
													   WHERE D.LEVEL_CD=@level_cd and H.ASSESS_TYPE=@ASSESS_TYPE and H.ASSESS_YEAR=@ASSESS_YEAR and H.WS_CD=@ws_cd );
					if @ori_score_final = 'E'  UPDATE TB_S_M_FOREIGN_MA_PEO SET OUT_REAL_E=OUT_REAL_E-1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and MA_TYPE=@MA_TYPE and MA_EMP_ID=@ma_emp_id  and 
											   GRP_CD=(SELECT H.GRP_CD FROM TB_S_M_FOREIGN_GROUP_H H join 
																		  TB_S_M_FOREIGN_GROUP_D D on H.ASSESS_TYPE=D.ASSESS_TYPE and H.ASSESS_YEAR =D.ASSESS_YEAR and H.GRP_CD=D.GRP_CD  
													   WHERE D.LEVEL_CD=@level_cd and H.ASSESS_TYPE=@ASSESS_TYPE and H.ASSESS_YEAR=@ASSESS_YEAR and H.WS_CD=@ws_cd );
					if @SCORE_DEPT = 'A'  UPDATE TB_S_M_FOREIGN_MA_PEO SET OUT_REAL_A=OUT_REAL_A+1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and MA_TYPE=@MA_TYPE and MA_EMP_ID=@ma_emp_id  and 
											   GRP_CD=(SELECT H.GRP_CD FROM TB_S_M_FOREIGN_GROUP_H H join 
																		  TB_S_M_FOREIGN_GROUP_D D on H.ASSESS_TYPE=D.ASSESS_TYPE and H.ASSESS_YEAR =D.ASSESS_YEAR and H.GRP_CD=D.GRP_CD  
													   WHERE D.LEVEL_CD=@level_cd and H.ASSESS_TYPE=@ASSESS_TYPE and H.ASSESS_YEAR=@ASSESS_YEAR and H.WS_CD=@ws_cd );
					if @SCORE_DEPT = 'B'  UPDATE TB_S_M_FOREIGN_MA_PEO SET OUT_REAL_B=OUT_REAL_B+1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and MA_TYPE=@MA_TYPE and MA_EMP_ID=@ma_emp_id  and 
											   GRP_CD=(SELECT H.GRP_CD FROM TB_S_M_FOREIGN_GROUP_H H join 
																		  TB_S_M_FOREIGN_GROUP_D D on H.ASSESS_TYPE=D.ASSESS_TYPE and H.ASSESS_YEAR =D.ASSESS_YEAR and H.GRP_CD=D.GRP_CD  
													   WHERE D.LEVEL_CD=@level_cd and H.ASSESS_TYPE=@ASSESS_TYPE and H.ASSESS_YEAR=@ASSESS_YEAR and H.WS_CD=@ws_cd );
					if @SCORE_DEPT = 'C'  UPDATE TB_S_M_FOREIGN_MA_PEO SET OUT_REAL_C=OUT_REAL_C+1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and MA_TYPE=@MA_TYPE and MA_EMP_ID=@ma_emp_id  and 
											   GRP_CD=(SELECT H.GRP_CD FROM TB_S_M_FOREIGN_GROUP_H H join 
																		  TB_S_M_FOREIGN_GROUP_D D on H.ASSESS_TYPE=D.ASSESS_TYPE and H.ASSESS_YEAR =D.ASSESS_YEAR and H.GRP_CD=D.GRP_CD  
													   WHERE D.LEVEL_CD=@level_cd and H.ASSESS_TYPE=@ASSESS_TYPE and H.ASSESS_YEAR=@ASSESS_YEAR and H.WS_CD=@ws_cd );
					if @SCORE_DEPT = 'D'  UPDATE TB_S_M_FOREIGN_MA_PEO SET OUT_REAL_D=OUT_REAL_D+1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and MA_TYPE=@MA_TYPE and MA_EMP_ID=@ma_emp_id  and 
											   GRP_CD=(SELECT H.GRP_CD FROM TB_S_M_FOREIGN_GROUP_H H join 
																		  TB_S_M_FOREIGN_GROUP_D D on H.ASSESS_TYPE=D.ASSESS_TYPE and H.ASSESS_YEAR =D.ASSESS_YEAR and H.GRP_CD=D.GRP_CD  
													   WHERE D.LEVEL_CD=@level_cd and H.ASSESS_TYPE=@ASSESS_TYPE and H.ASSESS_YEAR=@ASSESS_YEAR and H.WS_CD=@ws_cd );
					if @SCORE_DEPT = 'E'  UPDATE TB_S_M_FOREIGN_MA_PEO SET OUT_REAL_E=OUT_REAL_E+1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and MA_TYPE=@MA_TYPE and MA_EMP_ID=@ma_emp_id  and 
											   GRP_CD=(SELECT H.GRP_CD FROM TB_S_M_FOREIGN_GROUP_H H join 
																		  TB_S_M_FOREIGN_GROUP_D D on H.ASSESS_TYPE=D.ASSESS_TYPE and H.ASSESS_YEAR =D.ASSESS_YEAR and H.GRP_CD=D.GRP_CD  
													   WHERE D.LEVEL_CD=@level_cd and H.ASSESS_TYPE=@ASSESS_TYPE and H.ASSESS_YEAR=@ASSESS_YEAR and H.WS_CD=@ws_cd );
					**/
				END
				FETCH NEXT FROM cur_MA
				INTO
				   @ma_emp_id, @ma_type;
		END
		--關閉&釋放cursor
			CLOSE cur_MA;
			DEALLOCATE cur_MA;

		--更新TB_S_M_FOREIGN_DPT20_PEO
		/**
				if @is_out='N' BEGIN

					 print 'TB_S_M_FOREIGN_DPT20_PEO_N_OUT'; 
					 print @ASSESS_YEAR+'-'+@ASSESS_TYPE+'-'+@DEPT_NO_20+'-'+@ws_cd+'-'+@level_cd;
					

					if @ori_score_final = 'A'  UPDATE TB_S_M_FOREIGN_DEP20_PEO SET REAL_A=REAL_A-1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and DEPT_NO_20=@DEPT_NO_20 and 
												WS_CD=@ws_cd and CHARINDEX(@level_cd,SCORE_LEVEL_GROUP)>0 and IS_MERGER<>'A';
					if @ori_score_final = 'B'  UPDATE TB_S_M_FOREIGN_DEP20_PEO SET REAL_B=REAL_B-1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and DEPT_NO_20=@DEPT_NO_20 and 
												WS_CD=@ws_cd and CHARINDEX(@level_cd,SCORE_LEVEL_GROUP)>0 and IS_MERGER<>'A';
					if @ori_score_final = 'C'  UPDATE TB_S_M_FOREIGN_DEP20_PEO SET REAL_C=REAL_C-1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and DEPT_NO_20=@DEPT_NO_20 and 
												WS_CD=@ws_cd and CHARINDEX(@level_cd,SCORE_LEVEL_GROUP)>0 and IS_MERGER<>'A';
					if @ori_score_final = 'D'  UPDATE TB_S_M_FOREIGN_DEP20_PEO SET REAL_D=REAL_D-1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and DEPT_NO_20=@DEPT_NO_20 and 
												WS_CD=@ws_cd and CHARINDEX(@level_cd,SCORE_LEVEL_GROUP)>0 and IS_MERGER<>'A';
					if @ori_score_final = 'E'  UPDATE TB_S_M_FOREIGN_DEP20_PEO SET REAL_E=REAL_E-1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and DEPT_NO_20=@DEPT_NO_20 and 
												WS_CD=@ws_cd and CHARINDEX(@level_cd,SCORE_LEVEL_GROUP)>0 and IS_MERGER<>'A';
					if @SCORE_DEPT = 'A'  UPDATE TB_S_M_FOREIGN_DEP20_PEO SET REAL_A=REAL_A+1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and DEPT_NO_20=@DEPT_NO_20 and 
												WS_CD=@ws_cd and CHARINDEX(@level_cd,SCORE_LEVEL_GROUP)>0 and IS_MERGER<>'A';
					if @SCORE_DEPT = 'B'  UPDATE TB_S_M_FOREIGN_DEP20_PEO SET REAL_B=REAL_B+1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and DEPT_NO_20=@DEPT_NO_20 and 
												WS_CD=@ws_cd and CHARINDEX(@level_cd,SCORE_LEVEL_GROUP)>0 and IS_MERGER<>'A';
					if @SCORE_DEPT = 'C'  UPDATE TB_S_M_FOREIGN_DEP20_PEO SET REAL_C=REAL_C+1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and DEPT_NO_20=@DEPT_NO_20 and 
												WS_CD=@ws_cd and CHARINDEX(@level_cd,SCORE_LEVEL_GROUP)>0 and IS_MERGER<>'A';
					if @SCORE_DEPT = 'D'  UPDATE TB_S_M_FOREIGN_DEP20_PEO SET REAL_D=REAL_D+1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and DEPT_NO_20=@DEPT_NO_20 and 
												WS_CD=@ws_cd and CHARINDEX(@level_cd,SCORE_LEVEL_GROUP)>0 and IS_MERGER<>'A';
					if @SCORE_DEPT = 'E'  UPDATE TB_S_M_FOREIGN_DEP20_PEO SET REAL_E=REAL_E+1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and DEPT_NO_20=@DEPT_NO_20 and 
												WS_CD=@ws_cd and CHARINDEX(@level_cd,SCORE_LEVEL_GROUP)>0 and IS_MERGER<>'A';

				END
				ELSE BEGIN
					print 'TB_S_M_FOREIGN_DPT20_PEO_OUT'; 

					if @ori_score_final = 'A'  UPDATE TB_S_M_FOREIGN_DEP20_PEO SET OUT_REAL_A=OUT_REAL_A-1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and DEPT_NO_20=@DEPT_NO_20 and 
												WS_CD=@ws_cd and CHARINDEX(@level_cd,SCORE_LEVEL_GROUP)>0 and IS_MERGER<>'A';
					if @ori_score_final = 'B'  UPDATE TB_S_M_FOREIGN_DEP20_PEO SET OUT_REAL_B=OUT_REAL_B-1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and DEPT_NO_20=@DEPT_NO_20 and 
												WS_CD=@ws_cd and CHARINDEX(@level_cd,SCORE_LEVEL_GROUP)>0 and IS_MERGER<>'A';
					if @ori_score_final = 'C'  UPDATE TB_S_M_FOREIGN_DEP20_PEO SET OUT_REAL_C=OUT_REAL_C-1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and DEPT_NO_20=@DEPT_NO_20 and 
												WS_CD=@ws_cd and CHARINDEX(@level_cd,SCORE_LEVEL_GROUP)>0 and IS_MERGER<>'A';
					if @ori_score_final = 'D'  UPDATE TB_S_M_FOREIGN_DEP20_PEO SET OUT_REAL_D=OUT_REAL_D-1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and DEPT_NO_20=@DEPT_NO_20 and 
												WS_CD=@ws_cd and CHARINDEX(@level_cd,SCORE_LEVEL_GROUP)>0 and IS_MERGER<>'A';
					if @ori_score_final = 'E'  UPDATE TB_S_M_FOREIGN_DEP20_PEO SET OUT_REAL_E=OUT_REAL_E-1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and DEPT_NO_20=@DEPT_NO_20 and 
												WS_CD=@ws_cd and CHARINDEX(@level_cd,SCORE_LEVEL_GROUP)>0 and IS_MERGER<>'A';
					if @SCORE_DEPT = 'A'  UPDATE TB_S_M_FOREIGN_DEP20_PEO SET OUT_REAL_A=OUT_REAL_A+1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and DEPT_NO_20=@DEPT_NO_20 and 
												WS_CD=@ws_cd and CHARINDEX(@level_cd,SCORE_LEVEL_GROUP)>0 and IS_MERGER<>'A';
					if @SCORE_DEPT = 'B'  UPDATE TB_S_M_FOREIGN_DEP20_PEO SET OUT_REAL_B=OUT_REAL_B+1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and DEPT_NO_20=@DEPT_NO_20 and 
												WS_CD=@ws_cd and CHARINDEX(@level_cd,SCORE_LEVEL_GROUP)>0 and IS_MERGER<>'A';
					if @SCORE_DEPT = 'C'  UPDATE TB_S_M_FOREIGN_DEP20_PEO SET OUT_REAL_C=OUT_REAL_C+1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and DEPT_NO_20=@DEPT_NO_20 and 
												WS_CD=@ws_cd and CHARINDEX(@level_cd,SCORE_LEVEL_GROUP)>0 and IS_MERGER<>'A';
					if @SCORE_DEPT = 'D'  UPDATE TB_S_M_FOREIGN_DEP20_PEO SET OUT_REAL_D=OUT_REAL_D+1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and DEPT_NO_20=@DEPT_NO_20 and 
												WS_CD=@ws_cd and CHARINDEX(@level_cd,SCORE_LEVEL_GROUP)>0 and IS_MERGER<>'A';
					if @SCORE_DEPT = 'E'  UPDATE TB_S_M_FOREIGN_DEP20_PEO SET OUT_REAL_E=OUT_REAL_E+1 WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and DEPT_NO_20=@DEPT_NO_20 and 
												WS_CD=@ws_cd and CHARINDEX(@level_cd,SCORE_LEVEL_GROUP)>0 and IS_MERGER<>'A';

				END
				**/
		--更新TB_S_M_FOREIGN_TARGET
		
		SET @new_comments =@ori_comments + @COMMENTS;
		UPDATE  TB_S_M_FOREIGN_TARGET 
		SET SCORE_DEPT=@SCORE_DEPT, SCORE_FINAL=@SCORE_DEPT,  RECOMM_DESC=@RECOMM_DESC,
		    UPDATED_BY=@USERID, UPDATED_DT=GETDATE()
		WHERE ASSESS_TYPE=@ASSESS_TYPE and ASSESS_YEAR=@ASSESS_YEAR and EMP_ID=@EMP_ID;

		COMMIT TRANSACTION;

		EXECUTE dbo.SP_S_ASSESS_UPD_RO_DEP20_PEO @ASSESS_YEAR,@ASSESS_TYPE,@DEPT_NO_20,@USERID,@FUNCID;
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

END




