﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace LabelPrintingInterface.Reports
{
    public partial class TotalCurrentInventory : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void SearchImageButton1_Click(object sender, ImageClickEventArgs e)
        {
                string sKeyWord = this.SearchBox.Text;  //sKeyWord can be ASIN or FNSKU
                sKeyWord = sKeyWord.Trim();
                this.ListView1.DataSourceID = null;
                if (!string.IsNullOrEmpty(sKeyWord))
                    this.ObjectDataSource1.FilterExpression = "FNSKU LIKE " + "'%" + sKeyWord + "%'";
                else
                    this.ObjectDataSource1.FilterExpression = "";
                this.ObjectDataSource1.Select();
                this.ListView1.DataSource = this.ObjectDataSource1;

                this.ListView1.DataBind();

                if (this.ListView1.Items.Count == 0)
                {
                    this.ObjectDataSource1.FilterExpression = "[Product Name] LIKE " + "'%" + sKeyWord + "%'";
                    this.ObjectDataSource1.Select();
                    this.ListView1.DataSource = this.ObjectDataSource1;

                    this.ListView1.DataBind();
                }
        }

        private string RemoveExtraText(string value)
        {
            var allowedChars = "01234567890.,";
            return new string(value.Where(c => allowedChars.Contains(c)).ToArray());
        }

       

        protected void ListView1_PreRender(object sender, EventArgs e)
        {

        }

        protected void MaxiumRecordTextBox_OnTextChanged(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            try
            {
                this.DataPager1.PageSize = Convert.ToInt32(t.Text);
            }
            catch (Exception ex)
            {
                this.DataPager1.PageSize = 10;
            }
        }

        protected void DataPager1_PreRender(object sender, EventArgs e)
        {
            this.ListView1.DataSourceID = null;
            this.ListView1.DataSource = this.ObjectDataSource1;
            this.ListView1.DataBind();

            double dPageTotal = 0;
            double dPageInboundTotal = 0;
            double dPageFulfillableTotal = 0;
            foreach (ListViewDataItem Item in this.ListView1.Items)
            {
                //Label CostLabel = item.FindControl("CostLabel") as Label;
                //Label InboundLabel = item.FindControl("InboundLabel") as Label;
                Label InboundTotalLabel = Item.FindControl("InboundTotalLabel") as Label;
                //Label FulfillableLabel = item.FindControl("FulfillableLabel") as Label;
                Label FulfillableTotalLabel = Item.FindControl("FulfillableTotalLabel") as Label;
                Label SubTotalLabel = Item.FindControl("SubTotalLabel") as Label;

                //double dCost = Convert.ToDouble(RemoveExtraText(CostLabel.Text));
                double dInboundTotal = Convert.ToDouble(RemoveExtraText(InboundTotalLabel.Text));
                double dFulfillableTotal = Convert.ToDouble(RemoveExtraText(FulfillableTotalLabel.Text));
                //double dSubTotal = dInboundTotal + dFulfillableTotal;
                double dSubTotal = Convert.ToDouble(RemoveExtraText(SubTotalLabel.Text));
                dPageTotal += dSubTotal;
                dPageInboundTotal += dInboundTotal;
                dPageFulfillableTotal += dFulfillableTotal;

                //InboundTotalLabel.Text = dInboundTotal.ToString("C");
                //FulfillableTotalLabel.Text = dFulfillableTotal.ToString("C");
                //SubTotalLabel.Text = dSubTotal.ToString("C");
            }
            Label totalInboundLbl = (Label)this.ListView1.FindControl("TotalInboundLabel");
            Label totalFulfillableLbl = (Label)this.ListView1.FindControl("TotalFulfillableLabel");
            Label totalCountLbl = (Label)this.ListView1.FindControl("TotalCountLabel");

            totalInboundLbl.Text = dPageInboundTotal.ToString("C");
            totalFulfillableLbl.Text = dPageFulfillableTotal.ToString("C");
            totalCountLbl.Text = dPageTotal.ToString("C");
        }

        protected void ListView1tiesChanged(object sender, EventArgs e)
        {

        }
    }
}