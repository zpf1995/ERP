﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace AmbleAppServer.RfqMgr
{
   public class RfqMgr:System.MarshalByRefObject
    {
        DataClass.DataBase db = new DataClass.DataBase();

        public RfqMgr()
        { }


        public bool SaveRfq(Rfq rfq)
        {
        string strSql=string.Format("insert into rfq(customerName,partNo,salesId,contact,project,rohs,phone,fax,email,rfqdate,priority,dockdate,mfg,dc,custPartNo,genPartNo,alt,qty,packaging,targetPrice,resale,cost,firstPA,secondPA,rfqStates,infoToCustomer,infoToInternal,routingHistory) "
            +" values('{0}','{1}',{2},'{3}','{4}',{5},'{6}','{7}','{8}','{9}',{10},'{11}','{12}','{13}','{14}','{15}','{16}',{17},'{18}',{19},{20},{21},{22},{23},{24},'{25}','{26}','{27}')",
            rfq.customerName,rfq.partNo,rfq.salesId,rfq.contact,rfq.project,rfq.rohs,rfq.phone,rfq.fax,rfq.email,rfq.rfqdate.Date.ToShortDateString(),
            rfq.priority.HasValue?rfq.priority.Value.ToString():"null",rfq.dockdate.Date.ToShortDateString(),rfq.mfg,rfq.dc,rfq.custPartNo,rfq.genPartNo,rfq.alt,
            rfq.qty.HasValue?rfq.qty.Value.ToString():"null",rfq.packaging,rfq.targetPrice.HasValue?rfq.targetPrice.Value.ToString():"null",
            rfq.resale.HasValue?rfq.resale.Value.ToString():"null",rfq.cost.HasValue?rfq.cost.Value.ToString():"null",
            rfq.firstPA.HasValue?rfq.firstPA.Value.ToString():"null",rfq.secondPA.HasValue?rfq.secondPA.Value.ToString():"null",rfq.rfqStates,rfq.infoToCustomer,rfq.infoToInternal,rfq.routingHistory
            );
       
        int row=db.ExecDataBySql(strSql);
            if(row==1)
                return true;
            else
            return false;
        }

       public bool UpdateRfq(Rfq rfq)
       {
       string strSql=string.Format("update rfq set customerName={0},partNo={1},salesId={2},contact={3},project={4},rohs={5},phone={6},fax={7},email={8},rfqdate={9},priority={10},dockdate={11},mfg={12},dc={13},custPartNo={14},genPartNo={15},alt={16},qty={17},packaging={18},targetPrice={19},resale={20},cost={21},firstPA={22},secondPA={23},rfqStates={24}",
         rfq.customerName,rfq.partNo,rfq.salesId,rfq.contact,rfq.project,rfq.rohs,rfq.phone,rfq.fax,rfq.email,rfq.rfqdate.Date.ToShortDateString(),rfq.priority,rfq.dockdate.Date.ToShortDateString(),rfq.mfg,rfq.dc,rfq.custPartNo,rfq.genPartNo,rfq.alt,rfq.qty,rfq.packaging,rfq.targetPrice,rfq.resale,rfq.cost,rfq.firstPA,rfq.secondPA,rfq.rfqStates);
        
           int row=db.ExecDataBySql(strSql);
            if(row==1)
                return true;
            else
            return false;
       }


       public bool ChangeRfqState(int rfqState,int rfqId)
       {
           string strSql = string.Format("update rfq set rfqStates={0} where rfqNo={1}", rfqState, rfqId);
           if (db.ExecDataBySql(strSql) == 1)
               return true;
           return false;
       }

       public Rfq GetRfqAccordingToRfqId(int rfqId)
       {
           string strSql = string.Format("select * from rfq where rfqNo={0}", rfqId);
           DataTable dt = db.GetDataTable(strSql, "tempTable");
           DataRow dr = dt.Rows[0];
           Rfq rfq = new Rfq();

           rfq.customerName=dr["customerName"].ToString();
           rfq.partNo=dr["partNo"].ToString();
           rfq.salesId=Convert.ToInt32(dr["salesId"]);
           rfq.contact=dr["contact"].ToString();
           rfq.project=dr["project"].ToString();
           rfq.rohs=Convert.ToInt32(dr["rohs"]);
           rfq.phone=dr["phone"].ToString();
           rfq.fax=dr["fax"].ToString();
           rfq.email=dr["email"].ToString();
           rfq.rfqdate=Convert.ToDateTime(dr["rfqdate"]);
           if(dr["priority"]==DBNull.Value)
           {
               rfq.priority=null;
           }
           else
           {
            rfq.priority=Convert.ToInt32(dr["priority"]);
           }
           rfq.dockdate=Convert.ToDateTime(dr["dockdate"]);
           rfq.mfg=dr["mfg"].ToString();
           rfq.dc=dr["dc"].ToString();
           rfq.custPartNo=dr["custPartNo"].ToString();
           rfq.genPartNo=dr["genPartNo"].ToString();
           rfq.alt=dr["alt"].ToString();
           if(dr["qty"]==DBNull.Value)
               rfq.qty=null;
           else
               rfq.qty=Convert.ToInt32(dr["qty"]);

           rfq.packaging=dr["packaging"].ToString();

           if(dr["targetPrice"]==DBNull.Value)
               rfq.targetPrice=null;
           else
               rfq.targetPrice=Convert.ToSingle(dr["targetPrice"]);

            if(dr["cost"]==DBNull.Value)
               rfq.cost=null;
           else
               rfq.cost=Convert.ToSingle(dr["cost"]);

            if(dr["resale"]==DBNull.Value)
               rfq.resale=null;
           else
               rfq.resale=Convert.ToSingle(dr["resale"]);

            if(dr["firstPA"]==DBNull.Value)
               rfq.firstPA=null;
           else
               rfq.firstPA=Convert.ToInt32(dr["firstPA"]);
            if(dr["secondPA"]==DBNull.Value)
               rfq.secondPA=null;
           else
               rfq.secondPA=Convert.ToInt32(dr["secondPA"]);

           rfq.rfqStates=Convert.ToInt32(dr["rfqStates"]);
           rfq.infoToCustomer=dr["infoToCustomer"].ToString();
           rfq.infoToInternal=dr["infoToInternal"].ToString();
           rfq.routingHistory=dr["routingHistory"].ToString();

           if (dr["closeReason"] == DBNull.Value)
               rfq.closeReason = null;
           else
               rfq.closeReason = Convert.ToInt32(dr["closeReason"]);
          
           return rfq;
       }

        public int GetThePageCountOfDataTable(int itemsPerPage,int salesId,string filterColumn,string filterString)
        { int count=0;
                     //get the subs IDs include himself
           AmbleAppServer.AccountMgr.AccountMgr accountMgr = new AccountMgr.AccountMgr();

           List<int> subIds = accountMgr.GetAllSubsId(salesId);

           foreach (int id in subIds)
           {
               count += GetThePageCountOfDataTablePerSale(itemsPerPage, id, filterColumn, filterString);
           }
           return count;
        
        }

        public int GetThePageCountOfDataTablePerSale(int itemsPerPage,int salesId,string filterColumn,string filterString)
        { 
          //Get the account of dataset  of rfq
            string strSql;
            if ((!string.IsNullOrEmpty(filterColumn)) && (!(string.IsNullOrEmpty(filterString))))
            {
                strSql = string.Format("select count(*) from rfq where {0} like '%{1}%' and salesId={2}", filterColumn, filterString,salesId);

            }
            else

            {
                strSql = "select count(*) from rfq where salesId="+salesId;
            }
            int count = Convert.ToInt32(db.GetSingleObject(strSql));
            return (int)(Math.Ceiling((double)count / (double)itemsPerPage));
         
        }


        public DataTable GetMyRfqDataTableAccordingToPageNumber(int salesId, int pageNumber, int itemsPerPage, string filterColumn, string filterString)
        {
            string strSql;
           if ((!string.IsNullOrEmpty(filterColumn)) && (!(string.IsNullOrEmpty(filterString))))
            {
                strSql = string.Format("select * from rfq r left join rfqStateRecord rsr on r.rfqNo=rsr.rfqNo where {0} like '%{1}%' and salesId={2} limit {3},{4}",filterColumn,filterString,salesId, pageNumber * itemsPerPage, itemsPerPage);
     
            }   
           else
           {
               strSql= string.Format("select * from rfq r left join rfqStateRecord rsr on r.rfqNo=rsr.rfqNo where salesId={0} limit {1},{2}", salesId, pageNumber* itemsPerPage, itemsPerPage);
           }
            return  db.GetDataTable(strSql,"Table"+pageNumber);
        }

        public DataTable GetICanSeeRfqDataTableAccordingToPageNumber(int salesId, int pageNumber, int itemsPerPage, string filterColumn, string filterString)
        {
            AmbleAppServer.AccountMgr.AccountMgr accountMgr = new AccountMgr.AccountMgr();

            List<int> subIds = accountMgr.GetAllSubsId(salesId);

            StringBuilder sb = new StringBuilder();

            if ((!string.IsNullOrEmpty(filterColumn)) && (!(string.IsNullOrEmpty(filterString))))
            {
                sb.Append(string.Format("select * from rfq r left join rfqStateRecord rsr on r.rfqNo=rsr.rfqNo where {0} like '%{1}%' and ( salesId={2}",filterColumn,filterString,subIds[0]));
            }
            else
            {
                sb.Append("select * from rfq r left join rfqStateRecord rsr on r.rfqNo=rsr.rfqNo where ( salesId=" + subIds[0]);
            }
            
            if (subIds.Count() > 1)
            {
                for (int i = 1; i < subIds.Count(); i++)
                    sb.Append(" or salesId=" + subIds[i]);
             }
            sb.Append(string.Format(")  limit {0},{1}", pageNumber*itemsPerPage, itemsPerPage));

            return db.GetDataTable(sb.ToString(), "Table" + pageNumber);

        }

        public void CopyRfq(int rfqNo,int salesId)
        {
        //delete all the previous record
            string strSql=string.Format("delete from rfqCopy where salesId={0}",salesId);
            db.ExecDataBySql(strSql);
            strSql=string.Format("insert into rfqCopy values({0},{1})",salesId,rfqNo);
            db.ExecDataBySql(strSql);
        }

        public bool hasCopiedRfq(int salesId)
        {
         string strSql=string.Format("select count(*) from rfqCopy where salesId={0}",salesId);
         if((int)db.GetSingleObject(strSql)==1)
         {
          return true;
         }
         else
         {
        return false;
         }
        }

        public int GetRfqIdOfTheCopiedRecord(int salesId)
        {
            string strSql = string.Format("select rfqNo from rfqCopy where salesId={0}", salesId);
            return (int)db.GetSingleObject(strSql);

        }
      


    }
}