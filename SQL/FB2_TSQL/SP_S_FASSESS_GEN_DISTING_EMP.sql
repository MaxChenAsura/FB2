SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[SP_S_FASSESS_GEN_DISTING_EMP] 
@ASSESS_YEAR VARCHAR(4),
@ASSESS_TYPE VARCHAR(1),
@DEPT_NO VARCHAR(6), 
@USERID varchar(20),
@FUNCID varchar(30)
AS 
BEGIN
	SET NOCOUNT ON;
	--錯誤訊息
	DECLARE @pRowCount INT =0;
	DECLARE @pErr INT = 0;
	DECLARE @pErrMsg VARCHAR(1000) = NULL;
	
	DECLARE @proc_id_previous VARCHAR(60) = 'SP_S_FASSESS_GEN_DISTING_EMP';
	DECLARE @proc_id VARCHAR(60) = 'SP_S_FASSESS_GEN_DISTING_EMP';
	DECLARE @proc_desc NVARCHAR(120) = '特定區分人員生成';
	DECLARE @proc_scheduling VARCHAR(30) = 'CALL';
	DECLARE @proc_log NVARCHAR(600) = NULL;
	DECLARE @proc_status VARCHAR(1) = NULL;
	DECLARE @proc_y_cnt INT;

	--DECLARE @assess_type varchar(1);
	DECLARE @assess_sdt datetime;
	DECLARE @assess_edt datetime;
	DECLARE @is_valid varchar(1);
	DECLARE @is_out varchar(1);
	DECLARE @is_remark varchar(1);
	DECLARE @content varchar(100);
	DECLARE @emp_id varchar(5);
	DECLARE @disting_cd varchar(10);
	DECLARE @remark varchar(100);
	DECLARE @except_e varchar(1);
	DECLARE @limit_rate varchar(10);
	DECLARE @sm_remark varchar(1000);
	DECLARE @isSJ101 varchar(1);
	DECLARE @disting_desc varchar(60);
	DECLARE @datasource varchar(1);
	DECLARE @abs_score varchar(2);
	if(select CURSOR_STATUS('global','cur_assess_range'))>-3
	begin
		if(select CURSOR_STATUS('global','cur_assess_range'))>=0 CLOSE cur_assess_range;
		DEALLOCATE cur_assess_range;
	end
	if(select CURSOR_STATUS('global','cur_emp_id'))>-3
	begin
		if(select CURSOR_STATUS('global','cur_emp_id'))>=0 CLOSE cur_emp_id;
		DEALLOCATE cur_emp_id;
	end
	BEGIN TRANSACTION;
	BEGIN TRY
	DECLARE cur_assess_range CURSOR FOR
		SELECT  ASSESS_SDT, ASSESS_EDT
		FROM TB_S_M_FOREIGN_DATA
		WHERE ASSESS_YEAR=@ASSESS_YEAR and ASSESS_TYPE=@ASSESS_TYPE ;

	 --開啓CURSOR
    OPEN cur_assess_range;
	FETCH NEXT FROM cur_assess_range
	INTO
	      @assess_sdt, @assess_edt;
	--print @assess_sdt
	--print @assess_edt
	print '1';
	WHILE(@@FETCH_STATUS=0)
		BEGIN
		--刪除考課資料
		DELETE FROM TB_S_M_FOREIGN_DISTING_EMP where ASSESS_YEAR=@ASSESS_YEAR and ASSESS_TYPE=@assess_type and DATASOURCE='S';
		
		
		
		--GCC(SJ301)
		/**
		select @is_out=IS_OUT, @is_remark=IS_REMARK, @is_valid=IS_VALID, @content= CONTENT
		from TB_S_M_FOREIGN_DISTING
		where DISTING_CD='SJ301' 
		if @is_valid='Y'
			begin
				insert into TB_S_M_FOREIGN_DISTING_EMP
				select @ASSESS_YEAR as ASSESS_YEAR, @assess_type as ASSESS_TYPE ,A.EMP_ID											
					,'SJ301' as DISTING_CD ,'S' as DATASOURCE,@content REMARK,'' as ABS_SCORE, '' as CHG_WS_CD, '' as EXCEPT_E
					,@USERID,getDate(),@USERID,getDate(), @FUNCID										
				from  ( select EMP_ID 
				        from TB_H_R_EMP_TRANSFER 
						where TRANSFER_REASON ='B08' 
							  and ( START_DT  <= @assess_edt )  
							  and ( END_DT is null  or END_DT  between @assess_sdt and @assess_edt  or END_DT>=@assess_edt ) 									
						group by EMP_ID	) A							
					 inner join  (select EMP_ID  from TB_S_M_FOREIGN_TARGET where ASSESS_YEAR=@ASSESS_YEAR and ASSESS_TYPE=@assess_type ) B  on  A.EMP_ID  =B.EMP_ID								
			end;**/
		
		--借調(SJ302)
		select @is_out=IS_OUT, @is_remark=IS_REMARK, @is_valid=IS_VALID, @content= CONTENT
		from TB_S_M_FOREIGN_DISTING
		where DISTING_CD='SJ302' 
		if @is_valid='Y'
			begin
				insert into TB_S_M_FOREIGN_DISTING_EMP
				select @ASSESS_YEAR as ASSESS_YEAR, @assess_type as ASSESS_TYPE ,A.EMP_ID											
					,'SJ302' as DISTING_CD ,'S' as DATASOURCE,@content REMARK,'' as ABS_SCORE, '' as CHG_WS_CD, '' as EXCEPT_E
					,@USERID,getDate(),@USERID,getDate(), @FUNCID										
				from  ( select EMP_ID 
						from TB_H_R_EMP_TRANSFER 
						where TRANSFER_REASON ='B07'
							  and ( START_DT  <= @assess_edt )  	  
							  and ( END_DT is null  or END_DT  between @assess_sdt and @assess_edt or END_DT>=@assess_edt ) 								
						group by EMP_ID								
						) A	
					 inner join  (select EMP_ID  from TB_S_M_FOREIGN_TARGET where ASSESS_YEAR=@ASSESS_YEAR and ASSESS_TYPE=@assess_type ) B  on  A.EMP_ID  =B.EMP_ID								
				
			end;
		
		--留停-一般(SJ303)
		select @is_out=IS_OUT, @is_remark=IS_REMARK, @is_valid=IS_VALID, @content= CONTENT
		from TB_S_M_FOREIGN_DISTING
		where DISTING_CD='SJ303' 
		if @is_valid='Y'
			begin
				insert into TB_S_M_FOREIGN_DISTING_EMP
				select @ASSESS_YEAR as ASSESS_YEAR, @assess_type as ASSESS_TYPE ,A.EMP_ID											
					,'SJ303' as DISTING_CD ,'S' as DATASOURCE,@content REMARK,'' as ABS_SCORE, '' as CHG_WS_CD, '' as EXCEPT_E
					,@USERID,getDate(),@USERID,getDate(), @FUNCID										
				from  ( select EMP_ID 
						from TB_H_R_EMP_RETENTION 
						where HR_CHG_CD ='B11' 
							and ( START_DT  <= @assess_edt )  						
							and ( END_DT is null  or END_DT  between @assess_sdt and @assess_edt  or END_DT>=@assess_edt ) 						
							group by EMP_ID						
							) A	 inner join  
						(select EMP_ID  from TB_S_M_FOREIGN_TARGET where ASSESS_YEAR=@ASSESS_YEAR and ASSESS_TYPE=@assess_type ) B  on  A.EMP_ID  =B.EMP_ID								
				
			end;
		
		--留停-育嬰(SJ304)
		select @is_out=IS_OUT, @is_remark=IS_REMARK, @is_valid=IS_VALID, @content= CONTENT
		from TB_S_M_FOREIGN_DISTING
		where DISTING_CD='SJ304' 
		if @is_valid='Y'
			begin
				insert into TB_S_M_FOREIGN_DISTING_EMP
				select @ASSESS_YEAR as ASSESS_YEAR, @assess_type as ASSESS_TYPE ,A.EMP_ID											
					,'SJ304' as DISTING_CD ,'S' as DATASOURCE,@content REMARK,'' as ABS_SCORE, '' as CHG_WS_CD, '' as EXCEPT_E
					,@USERID,getDate(),@USERID,getDate(), @FUNCID										
				from  ( select EMP_ID 
						from TB_H_R_EMP_RETENTION 
						where HR_CHG_CD ='B12'  
							and ( START_DT <= @assess_edt )  							
							and ( END_DT is null  or END_DT  between @assess_sdt and @assess_edt  or END_DT>=@assess_edt ) 						
							group by EMP_ID						
							) A	 inner join  
						(select EMP_ID  from TB_S_M_FOREIGN_TARGET where ASSESS_YEAR=@ASSESS_YEAR and ASSESS_TYPE=@assess_type ) B  on  A.EMP_ID  =B.EMP_ID								
				
			end
		
		--獎懲(SJ501)
		select @is_out=IS_OUT, @is_remark=IS_REMARK, @is_valid=IS_VALID, @content= CONTENT
		from TB_S_M_FOREIGN_DISTING
		where DISTING_CD='SJ501' 
		if @is_valid='Y'
			begin
				insert into TB_S_M_FOREIGN_DISTING_EMP
				select @ASSESS_YEAR as ASSESS_YEAR, @assess_type as ASSESS_TYPE ,EMP_ID											
					,'SJ501' as DISTING_CD ,'S' as DATASOURCE
					,iif(THIRD_CNT_P=0,'','嘉獎'+convert(varchar,THIRD_CNT_P)+'回' )							
					+iif(SECOND_CNT_P=0,'','小功'+convert(varchar,SECOND_CNT_P)+'回' )							
					+iif(FIRST_CNT_P=0,'','大功'+convert(varchar,FIRST_CNT_P)+'回' )							
					+iif(THIRD_CNT_M=0,'','申誡'+convert(varchar,THIRD_CNT_M)+'回' )							
					+iif(SECOND_CNT_M=0,'','小過'+convert(varchar,SECOND_CNT_M)+'回' )							
					+iif(FIRST_CNT_M=0,'','大過'+convert(varchar,FIRST_CNT_M)+'回' ) as REMARK	
					,'' as ABS_SCORE, '' as CHG_WS_CD, '' as EXCEPT_E
					,@USERID,getDate(),@USERID,getDate(), @FUNCID										
				from TB_S_M_FOREIGN_TARGET 							
				where ASSESS_YEAR=@ASSESS_YEAR and ASSESS_TYPE=@assess_type							
					and THIRD_CNT_P 								
					+SECOND_CNT_P								
					+FIRST_CNT_P								
					+THIRD_CNT_M								
					+SECOND_CNT_M								
					+FIRST_CNT_M  >0							
				
			end;
		
		
		
		
		--絕對E考課(SJ701)
		select @is_out=IS_OUT, @is_remark=IS_REMARK, @is_valid=IS_VALID, @content= CONTENT
		from TB_S_M_FOREIGN_DISTING
		where DISTING_CD='SJ701' 
		if @is_valid='Y'
			begin
				insert into TB_S_M_FOREIGN_DISTING_EMP
				select @ASSESS_YEAR as ASSESS_YEAR, @assess_type as ASSESS_TYPE ,EMP_ID											
					,'SJ701' as DISTING_CD ,'S' as DATASOURCE
					,@content REMARK	
					,'' as ABS_SCORE, '' as CHG_WS_CD, '' as EXCEPT_E
					,@USERID,getDate(),@USERID,getDate(), @FUNCID										
				from TB_S_M_FOREIGN_TARGET 							
				where ASSESS_YEAR=@ASSESS_YEAR and ASSESS_TYPE=@assess_type							
					and (FIRST_CNT_M>=1 or SECOND_CNT_M>=2 or	LEAVE_Q>=3 or (LEAVE_A+LEAVE_B)>=30)						
				
			end;

		FETCH NEXT FROM cur_assess_range
		INTO
			 @assess_sdt, @assess_edt;
		END;
		CLOSE cur_assess_range;
		DEALLOCATE cur_assess_range;
	--回寫備考內容
	DECLARE cur_emp_id CURSOR FOR
		SELECT EMP_ID
		FROM TB_S_M_FOREIGN_DISTING_EMP
		WHERE ASSESS_YEAR=@ASSESS_YEAR and ASSESS_TYPE=@ASSESS_TYPE
		GROUP BY ASSESS_TYPE,EMP_ID;
	 --開啓CURSOR
    OPEN cur_emp_id;
	FETCH NEXT FROM cur_emp_id
	INTO
	     @emp_id;
	WHILE(@@FETCH_STATUS=0)
		BEGIN
			DECLARE cur_disting_emp CURSOR FOR
				SELECT  A.DISTING_CD, A.REMARK,B.DISTING_DESC,A.EXCEPT_E,B.IS_OUT,A.ABS_SCORE,A.DATASOURCE
				FROM TB_S_M_FOREIGN_DISTING_EMP A join
					 TB_S_M_FOREIGN_DISTING B on A.DISTING_CD=B.DISTING_CD
				WHERE ASSESS_YEAR=@ASSESS_YEAR and ASSESS_TYPE=@assess_type and EMP_ID=@emp_id;
			OPEN cur_disting_emp;
			FETCH NEXT FROM cur_disting_emp
			INTO
				@disting_cd, @remark, @disting_desc,@except_e, @is_out,@abs_score, @datasource;
			set @sm_remark='';

			WHILE(@@FETCH_STATUS=0)
				BEGIN
					
					
					if(@is_out='Y')
					begin
						 update TB_S_M_FOREIGN_TARGET set IS_OUT ='Y'
						 where ASSESS_YEAR=@ASSESS_YEAR and ASSESS_TYPE=@assess_type and EMP_ID=@emp_id;
					end
					if (@sm_remark<>'')set @sm_remark=@sm_remark+';';
					if(@datasource='U')
					begin
					   if @remark='' set @sm_remark=@sm_remark+@disting_desc;
					   if @remark<>'' set @sm_remark=@sm_remark+@disting_desc+';'+@remark;
						
					end
					if(@datasource<>'U')
					begin
						set @sm_remark=@sm_remark+@remark;
					end
					set @limit_rate='';
					if @ASSESS_TYPE='2'
					BEGIN
						if (@disting_cd='SJ701')
						begin
							set @limit_rate='E';
							--if (@sm_remark<>'')set @sm_remark=@sm_remark+';';
							--set @sm_remark=@sm_remark+'絕對E考課';
						end
					END
					
					if (@except_e='Y')
					begin
						set @limit_rate='A,B,C,D';
						if (@sm_remark<>'')set @sm_remark=@sm_remark+';';
						set @sm_remark=@sm_remark+'E考課除外';
					end
					if(@datasource='U')
					begin
						if @abs_score<>'' set @limit_rate=@abs_score;						
						--if (@sm_remark<>'')set @sm_remark=@sm_remark+';';
						--set @sm_remark=@sm_remark+'絶對'+@abs_score+'考課';
					end;
					if(@limit_rate<>'')
					begin 
						 update TB_S_M_FOREIGN_TARGET set LIMIT_RATE=@limit_rate
						 where ASSESS_YEAR=@ASSESS_YEAR and ASSESS_TYPE=@assess_type and EMP_ID=@emp_id;

						 if len(@limit_rate)=1
							begin
							update TB_S_M_FOREIGN_TARGET set  SCORE_DIRC=@limit_rate, SCORE_DEPT=@limit_rate, SCORE_FINAL=@limit_rate
									where ASSESS_YEAR=@ASSESS_YEAR and ASSESS_TYPE=@assess_type and EMP_ID=@emp_id;
							end;
					end;
					FETCH NEXT FROM cur_disting_emp
					INTO
						@disting_cd, @remark, @disting_desc,@except_e, @is_out,@abs_score, @datasource;
				END;
			 --更新該員工的TTB_S_M_FOREIGN_TARGET
			 --print @emp_id+';'+@sm_remark+';'+@limit_rate+';'+@is_out;
			 update TB_S_M_FOREIGN_TARGET set DISTING_REMARK =@sm_remark
			 where ASSESS_YEAR=@ASSESS_YEAR and ASSESS_TYPE=@assess_type and EMP_ID=@emp_id;

			CLOSE cur_disting_emp;
			DEALLOCATE cur_disting_emp;
		
		FETCH NEXT FROM cur_emp_id
		INTO
			  @emp_id;

		END;		
		CLOSE cur_emp_id;
		DEALLOCATE cur_emp_id;

		COMMIT TRANSACTION;
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
