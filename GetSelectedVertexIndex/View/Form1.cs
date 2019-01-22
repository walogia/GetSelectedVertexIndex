
///////////////////////////////////////////////////////////////////////////
//DO WHAT THE FUCK YOU WANT TO PUBLIC LICENSE
//                   Version 2, December 2004
// 
//Copyright (C) 2004 Sam Hocevar <sam@hocevar.net>
//
//Everyone is permitted to copy and distribute verbatim or modified
//copies of this license document, and changing it is allowed as long
//as the name is changed.
// 
//           DO WHAT THE FUCK YOU WANT TO PUBLIC LICENSE
//  TERMS AND CONDITIONS FOR COPYING, DISTRIBUTION AND MODIFICATION
// 
// 0. You just DO WHAT THE FUCK YOU WANT TO.
///////////////////////////////////////////////////////////////////////////
using PEPlugin;
using PEPlugin.Pmx;
using PEPlugin.View;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


namespace View
{
	/// <summary>
	/// Description of Form1.
	/// </summary>
	public partial class Form1 : Form
	{
		private IPERunArgs args;
		private IPEPluginHost host;
		private IPEConnector connect;
		private IPXPmx PMX;
		public IPXPmxViewConnector PMXView;
		public IPEPMDViewConnector PMDView;
		public IPEBuilder builder;
		public IPEShortBuilder bd;
        
        public void SetHostArgs(IPERunArgs args)
        {
            this.args = args;
        }
                
		public Form1()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		void Form1FormClosing(object sender, FormClosingEventArgs e)
		{
			 if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                // フォームを非表示設定
                this.Visible = false;
            }
		}
		
		private void InitializeValue()
		{
			try
			{
				this.host = this.args.Host;;
				this.connect = this.host.Connector;
				this.PMX = this.connect.Pmx.GetCurrentState();
				this.PMDView = this.connect.View.PMDView;
				this.PMXView = this.connect.View.PmxView;
				this.builder = this.host.Builder;
				this.bd = this.host.Builder.SC;
			}
			catch
			{
				MessageBox.Show("值初始化失败");
			}
		}

	        /// <summary>
	        /// モデル・画面を更新します。
	        /// </summary>
	        public void Update()
	        {
	            this.connect.Pmx.Update(this.PMX);
	            this.connect.Form.UpdateList(PEPlugin.Pmd.UpdateObject.All);
	            this.connect.View.PMDView.UpdateModel();
	            this.connect.View.PMDView.UpdateView();
	        }
		


		//*******************************主要函数开始***************************//
		/// <summary>
        /// 获取頂点Index
        /// </summary>
        /// <param name="Vertex"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public int GetVertexIndex(IPXVertex Vertex, int e = 0)
        {
            for (int i = 0; i < this.PMX.Vertex.Count; i++)
            {
                if (this.PMX.Vertex[i] == Vertex)
                {
                    return i;
                }
            }
            if (e == 0)
            {
                return -1;
            }
            else
            {
                throw new System.Exception("Indexを取得できません。");
            }
        }
        
	    /// <summary>
        /// PMXViewで選択した頂点のリストを返します。
        /// </summary>
        /// <returns></returns>
        public List<IPXVertex> GetSelectedVertexList()
        {
            int[] SelectedItem = this.PMDView.GetSelectedVertexIndices();
            if (SelectedItem.Length == 0)
            {
                throw new System.Exception("没有选中顶点");
            }
            List<IPXVertex> RetList = new List<IPXVertex>();
            for (int i = 0; i < SelectedItem.Length; i++)
            {
                RetList.Add(this.PMX.Vertex[SelectedItem[i]]);
            }
            return RetList;
        }
		//*******************************主要函数结束***************************//
		//***************************************全局快捷键设置********************************//
		//此下三处，参考自 https://www.cnblogs.com/Randy0528/archive/2013/02/04/2892062.html 实验通过
		void Form1Activated(object sender, EventArgs e)
		{
		//在FormA的Activate事件中注册热键，本例中注册Shift+S，Ctrl+Z，Alt+D这三个热键。这里的Id号可任意设置，但要保证不被重复。
			//注册热键Shift+S，Id号为100。HotKey.KeyModifiers.Shift也可以直接使用数字4来表示。
		    HotKey.RegisterHotKey(Handle, 100, HotKey.KeyModifiers.Shift, Keys.S); 
		    //注册热键Ctrl+B，Id号为101。HotKey.KeyModifiers.Ctrl也可以直接使用数字2来表示。
		 //   HotKey.RegisterHotKey(Handle, 101, HotKey.KeyModifiers.Ctrl, Keys.B);
		    //注册热键Alt+D，Id号为102。HotKey.KeyModifiers.Alt也可以直接使用数字1来表示。
	   	//   HotKey.RegisterHotKey(Handle, 102, HotKey.KeyModifiers.Alt, Keys.D);
		}
		void Form1Leave(object sender, EventArgs e)
		{
	    //注销Id号为100的热键设定
	    HotKey.UnregisterHotKey(Handle, 100);
	    //注销Id号为101的热键设定
	    HotKey.UnregisterHotKey(Handle, 101);
	    //注销Id号为102的热键设定
	    HotKey.UnregisterHotKey(Handle, 102);
		}
		
		//重载FromA中的WndProc函数
		/// 
		/// 监视Windows消息
		/// 重载WndProc方法，用于实现热键响应
		/// 
		/// 
		protected override void WndProc(ref Message m)
		{
		    const int WM_HOTKEY = 0x0312;
		    //按快捷键 
		    switch (m.Msg)
		    {
		        case WM_HOTKEY:
		            switch (m.WParam.ToInt32())
		            {
		                case 100:    //按下的是Shift+S
		                    //此处填写快捷键响应代码  
		             	    this.InitializeValue();
							//在此编写代码
							List<IPXVertex> list=GetSelectedVertexList();
								//获取选中顶点数
								int i=list.Count;
								string k="";
								foreach (IPXVertex v in list)  
								  {
								
									k+=GetVertexIndex(v).ToString()+"\r\n";
								
								  }
								textBox1.Text=k;
						
						this.Update();
		                    break;
//		                case 101:    //按下的是Ctrl+B
//		                    //此处填写快捷键响应代码
//		                    break;
//		                case 102:    //按下的是Alt+D
//		                    //此处填写快捷键响应代码
//		                    break;
		            }
		        break;
		    }
		    base.WndProc(ref m);
		}
		//*********************截止到此********************//
	}
}
