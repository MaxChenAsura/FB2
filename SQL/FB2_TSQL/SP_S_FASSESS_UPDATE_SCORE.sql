USE [FB2DB]
GO

/****** Object:  StoredProcedure [dbo].[SP_S_ASSESS_UPDATE_SCORE]    Script Date: 2026/7/29 下午 03:20:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Steven Su 
-- Create date: 2014/08/11
-- Update date: 2020/03/16
-- 20170930 能力預設為C;業績晉昇預設為B,其餘為C
-- 20191210 update時,條件忘記加@
-- 20200316 能力考核 2S考績為空白,其餘C / 業績考核 為C
-- Description:	考績一括維護- 考核年度、考核類別,使用者id,FunctionID
-- =============================================

CREATE procedure [dbo].[SP_S_FASSESS_UPDATE_SCORE]
	@ASSESS_YEAR varchar(4), 
	@ASSESS_TYPE varchar(1), 
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
	
	DECLARE @proc_id_previous VARCHAR(60) = 'SP_S_FASSESS_UPDATE_SCORE';
	DECLARE @proc_id VARCHAR(60) = 'SP_S_FASSESS_UPDATE_SCORE';
	DECLARE @proc_desc NVARCHAR(120) = '考績一括維護';
	DECLARE @proc_scheduling VARCHAR(30) = 'CALL';
	DECLARE @proc_log NVARCHAR(600) = NULL;
	DECLARE @proc_status VARCHAR(1) = NULL;
	DECLARE @proc_y_cnt INT;

	--宣告變數
	declare @strSQL nvarchar(4000);  --一定要nvarchar
	declare @strSQL1 nvarchar(2000);  --一定要nvarchar,

	--除外對象的條件
	declare @ASSESS_SDT varchar(10) --考核起始日期
	declare @ASSESS_EDT varchar(10) --考核結束日期

	declare @exam_days      decimal(3,0);     --試用期滿日
	declare @WK_EMP_JOIN_DT    varchar(10);   --WK_正社員日-轉正社員日或入社日[除外對象]
	declare @WK_LEAVE_DT		varchar(10);  --WK_離社日-考核對象的離社日期[除外對象用]
	declare @WK_YEAR_DT		varchar(10);  --WK_年資日-計算資格年資及入社年資用
	declare @WK_AGE_DT		varchar(10);  --WK_年齡日-計算年齡用

	--20171002
	IF @ASSESS_TYPE='1'
		BEGIN
			SET @ASSESS_SDT = CONVERT(varchar(4),YEAR(GETDATE()-1)) + '/04/01';
			SET @ASSESS_EDT = CONVERT(varchar(4),YEAR(GETDATE())) + '/03/31';
			SET @WK_LEAVE_DT = CONVERT(varchar(4),YEAR(GETDATE())) + '/07/01';
			SET @WK_YEAR_DT = CONVERT(varchar(4),YEAR(GETDATE())) + '/06/30';
			SET @WK_AGE_DT = CONVERT(varchar(4),YEAR(GETDATE()-1)) + '/03/31';
		END
	ELSE
		BEGIN
			SET @ASSESS_SDT = CONVERT(varchar(4),YEAR(GETDATE())) + '/04/01';
			SET @ASSESS_EDT = CONVERT(varchar(4),YEAR(GETDATE())) + '/11/30';
			SET @WK_LEAVE_DT = CONVERT(varchar(4),YEAR(GETDATE()) +1 ) + '/01/01';
			SET @WK_YEAR_DT = CONVERT(varchar(4),YEAR(GETDATE())) + '/12/31';
			SET @WK_AGE_DT = CONVERT(varchar(4),YEAR(GETDATE()-1)) + '/12/31';
		END
	--從人事資料維護檔  取出的參數
	declare @cur_EMP_ID						varchar(5)	;	--工號
	declare @cur_EMP_NAME					nvarchar(30);	--姓名
	declare @cur_LEVEL_CD					varchar(3);		--資格代號 
	declare @cur_PJOB_CD					varchar(4);		--職務代號
	declare @cur_PJOB_DESC					nvarchar(30);	--職務名稱
	declare @cur_AGE						decimal(3);     --年齡
	declare @cur_LEVELUP_FLAG               varchar(1);     --當年昇格
	declare @dept_flag				varchar(1);		--跨部異動(跨部註記)

	declare @cur_GRADE_CD					varchar(1);		--級數
	declare @wk_levelUP_3A decimal(1,0) ;     -- 是否有昇格註記(3A含以上)
	declare @wk_levelUP_3B decimal(1,0) ;     -- 是否有昇格註記(3B含以上)
	declare @wk_levelUP_year varchar(4);   --昇格註記的年度,判斷用
	declare @assess_year_decimal decimal(4,0)=convert(decimal(4,0),@ASSESS_YEAR);

	IF @ASSESS_TYPE='1'
		BEGIN
			SET @wk_levelUP_year = CONVERT(varchar(4),@assess_year_decimal);
		END
	ELSE
		BEGIN
			SET @wk_levelUP_year = CONVERT(varchar(4),@assess_year_decimal +1 )
		END
	--要更新的考績
	declare @wk_SCORE varchar(2);
	declare @approve_status varchar(1) ='N';

	--其它
	declare @wk_levelCount decimal(4) ;             --判斷用
	Declare @sysDate DateTime  = CURRENT_TIMESTAMP; --更新日期時間
	declare @gen_dt DateTime = CAST(CURRENT_TIMESTAMP AS DATE) ;    --對象生成日
	
	--WK_正社員日-轉正社員日或入社日[除外對象]
	SET @strSQL1 = ' select '
					 +'  EMP_ID, EMP_NAME  '
					 +', LEVEL_CD, GRADE_CD, PJOB_CD, PJOB_DESC, AGE,LEVELUP_FLAG '
					 +' from TB_S_M_FOREIGN_TARGET '
					 +' where 1=1 '
					 +' and ASSESS_YEAR= ' + @ASSESS_YEAR
					 +' and ASSESS_TYPE=' +@ASSESS_TYPE 

	--取得符合條件的員工
	SET @strSQL = ' declare cur_AccessTaget CURSOR FOR '
				  + @strSQL1;

	--print @strSQL
	exec sp_executesql @strSQL;

	BEGIN TRANSACTION UPDATE_TB_S_ASSESS
	BEGIN TRY
		--開啟CUROSR
		OPEN cur_AccessTaget;
		--逐筆讀取並處理
		FETCH NEXT FROM cur_AccessTaget 
		INTO  @cur_EMP_ID, @cur_EMP_NAME, @cur_LEVEL_CD, @cur_GRADE_CD, @cur_PJOB_CD, @cur_PJOB_DESC, @cur_AGE,  @cur_LEVELUP_FLAG	
					
		WHILE(@@FETCH_STATUS=0)
			BEGIN
				SET @wk_SCORE ='C';
				 
				--20200316能力考核及2S人員,全部預設為空白
				if @ASSESS_TYPE ='1' AND @cur_LEVEL_CD='2S'
					SET @wk_SCORE ='';

				/*
				--業績考核,晉昇為B,其餘預設為C
				if @ASSESS_TYPE ='2' and dbo.FN_S_IS_PROMOTION(@wk_levelUP_year,@cur_EMP_ID,'0') ='Y'
				BEGIN
					--判斷是否有晉昇(不含晉級)，
					SET @cur_LEVELUP_FLAG ='V';
				END
				*/

				/*20200316
				--業績考課及晉昇
				if @ASSESS_TYPE ='2'  and @cur_LEVELUP_FLAG='V'
				BEGIN
					print '晉昇考績為B'
					--晉昇為B
					SET @wk_SCORE ='B';

					--業績考核時,用新的資格代號
					select 
					 @cur_LEVEL_CD =LEVEL_CD_NEW
					,@cur_GRADE_CD=GRADE_CD_NEW 
					,@cur_PJOB_CD=PJOB_CD_NEW
					,@cur_PJOB_DESC= PJOB_DESC_NEW
					from TB_S_M_PROMOTION_TXN b  with (nolock)
					where b.EMP_ID=@cur_EMP_ID 
					and b.DATA_YEAR=@wk_levelUP_year;

				END
				*/
				

				/*
				--20160113 因為有可能先對象生成後,晉陞名單才完成,故進行考績一括維護時,重新判斷是否晉陞
				--昇格註記 判斷是否有3B(含)以下(僅有業績考課昇核註記為V)
				select @wk_levelUP_3B =count(0) from  TB_S_M_PROMOTION_TXN b  with (nolock)
				left join VW_TB_H_M_LEVEL c on b.LEVEL_CD_NEW = c.LEVEL_CD
				where b.EMP_ID=@cur_EMP_ID  and b.PROCESS_STATUS='Y'
				and b.DATA_YEAR=@wk_levelUP_year
				and b.LEVEL_CD_NEW not in ('5A','RB')
				and c.ORDER_SEQ >= (
					select ORDER_SEQ from VW_TB_H_M_LEVEL  with (nolock)
					where LEVEL_CD = '3B')

				IF @wk_levelUP_3B >0 and @ASSESS_TYPE ='2'
					BEGIN
						SET @cur_LEVELUP_FLAG ='V';


					END
				ELSE
					BEGIN
						SET @cur_LEVELUP_FLAG =''
						
						--3A(含)以上的昇格 --(不論是何考課,昇核註記為V)
						select @wk_levelUP_3A =count(0) from  TB_S_M_PROMOTION_TXN b  with (nolock)
						left join VW_TB_H_M_LEVEL c on b.LEVEL_CD_NEW = c.LEVEL_CD
						where b.EMP_ID=@cur_EMP_ID  and b.PROCESS_STATUS='Y'
						 and b.DATA_YEAR=@wk_levelUP_year
						 and b.LEVEL_CD_NEW not in ('5A','RB')
						 and c.ORDER_SEQ <= (
							select ORDER_SEQ from VW_TB_H_M_LEVEL  with (nolock)
							where LEVEL_CD = '3A')

						IF @wk_levelUP_3A =0 
							BEGIN
								SET @cur_LEVELUP_FLAG ='';
							END
						ELSE
							BEGIN
								SET @cur_LEVELUP_FLAG ='V';
							END
					END
			
				--業績考核時,用新的資格代號
				IF @ASSESS_TYPE ='2' and @cur_LEVELUP_FLAG='V'
					BEGIN
						 select @cur_LEVEL_CD =LEVEL_CD_NEW, @cur_GRADE_CD=GRADE_CD_NEW 
						 ,@cur_PJOB_CD=PJOB_CD_NEW, @cur_PJOB_DESC= PJOB_DESC_NEW
						 from TB_S_M_PROMOTION_TXN b  with (nolock)
						 where b.EMP_ID=@cur_EMP_ID 
						   and b.DATA_YEAR=@wk_levelUP_year;
					END

				--若資格>=2B, 則為C2,其餘為C
				select @wk_levelCount =  count(0) 
				from VW_TB_H_M_LEVEL
				where ORDER_SEQ <=
				(
					select order_SEQ from VW_TB_H_M_LEVEL where LEVEL_CD ='2B'
				)
				and LEVEL_CD =@cur_LEVEL_CD

				IF @wk_levelCount > 0 
					BEGIN
						SET @wk_SCORE ='C2'
					END
				ELSE
					BEGIN
						SET @wk_SCORE ='C'
					END

				--資格>=3A and 當年昇格='V' => 能力及業績考績為 D
				select @wk_levelCount =  count(0) from VW_TB_H_M_LEVEL
				where ORDER_SEQ <=
				(
					select order_SEQ from VW_TB_H_M_LEVEL where LEVEL_CD ='3A'
				)
				and LEVEL_CD =@cur_LEVEL_CD

				IF @wk_levelCount > 0  and @cur_LEVELUP_FLAG='V'
					BEGIN
						SET @wk_SCORE ='D'
					END

				--資格<=3B and 當年昇格='V' => 業績考績為D
				select @wk_levelCount =  count(0) from VW_TB_H_M_LEVEL
				where ORDER_SEQ >=
				(
					select order_SEQ from VW_TB_H_M_LEVEL where LEVEL_CD ='3B'
				)
				and LEVEL_CD =@cur_LEVEL_CD

				IF @wk_levelCount > 0  and @cur_LEVELUP_FLAG='V'and @ASSESS_TYPE='2'
					BEGIN
						SET @wk_SCORE ='D'
					END
				--*/

				--年齡>=55 and 非管理者(職務代號<> M 開頭) => 能力及業績考績為 D
				/*201601118 不需要
				IF @cur_AGE >=55  and left(@cur_PJOB_CD,1) <>'M' 
					BEGIN
						SET @wk_SCORE ='D'
					END
				*/

				update TB_S_M_FOREIGN_TARGET
				set  SCORE_DEPT = @wk_SCORE
					,SCORE_FINAL = @wk_SCORE
					,SCORE_FINAL_FLAG =''
					,SCORE_FLAG = ''
					,APPROVE_MARK = ''
					,LEVEL_CD = @cur_LEVEL_CD
					,GRADE_CD = @cur_GRADE_CD
					--,LEVELUP_FLAG = @cur_LEVELUP_FLAG 
					,PJOB_CD = @cur_PJOB_CD
					,PJOB_DESC = @cur_PJOB_DESC
					,UPDATED_BY =@USERID
					,UPDATED_DT = @sysDate 
					,FUNC_ID = @FUNCID
				where ASSESS_YEAR = @ASSESS_YEAR
				and ASSESS_TYPE = @ASSESS_TYPE
				and EMP_ID = @cur_EMP_ID


				FETCH NEXT FROM cur_AccessTaget 
				INTO  @cur_EMP_ID, @cur_EMP_NAME, @cur_LEVEL_CD, @cur_GRADE_CD, @cur_PJOB_CD, @cur_PJOB_DESC, @cur_AGE,  @cur_LEVELUP_FLAG	
			END


		CLOSE cur_AccessTaget
		DEALLOCATE cur_AccessTaget
		
	--更新考核資料維護檔(變成未簽核)
	update TB_S_M_FOREIGN_DATA
	set  
		  GEN_DT = @gen_dt
		, RELEASE_DT=null
		, RELEASE_BY=''
		, APPROVE_DT=null
		, APPROVE_BY=''
		, APPROVE_STATUS='N'
		, ASSESS_RELEASE_DT=null
		, ASSESS_RELEASE_BY=''
		--, REMARK =''
		, FREEZE_FLAG ='N'
		,UPDATED_BY =@USERID, UPDATED_DT = @sysDate ,FUNC_ID = @FUNCID
	where ASSESS_YEAR = @ASSESS_YEAR
	and ASSESS_TYPE = @ASSESS_TYPE
		
			
	print '執行完畢';
	
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION UPDATE_TB_S_ASSESS;
		SELECT @pErr = ERROR_NUMBER(), @pErrMsg = '[TB_H_R_SP_LOG:INSERT]'+ERROR_MESSAGE();
		GOTO WriteLog;
	END CATCH
	COMMIT TRANSACTION UPDATE_TB_S_ASSESS;
	IF @@ERROR <> 0 GOTO WriteLog;


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
			VALUES(@proc_id, @ASSESS_SDT, @proc_desc, NULL, CURRENT_TIMESTAMP, @proc_status, @proc_log, @USERID, @FUNCID);
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION ADD_TB_H_R_SP_LOG;
			SELECT @pErr = ERROR_NUMBER(), @pErrMsg = '[ADD_TB_H_R_SP_LOG:INSERT]:' + ERROR_MESSAGE();
		END CATCH
	COMMIT TRANSACTION ADD_TB_H_R_SP_LOG;

	RETURN (@pErr)


END




GO


