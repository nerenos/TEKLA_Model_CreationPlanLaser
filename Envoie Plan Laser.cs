using Tekla.Structures;
using TSM = Tekla.Structures.Model;
using Tekla.Structures.Model.Operations;

using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Tekla.Technology.Akit.UserScript {
    public class Script {
        
        public static void Run(Tekla.Technology.Akit.IScript akit) {
			
			TSM.Model Model = new TSM.Model();
			TSM.ModelInfo Info = Model.GetInfo();
			if (Model.GetConnectionStatus())
			{
				if (Info.ModelName.Length == 0)
				{
					MessageBox.Show("A Tekla Structures model is not open.");
				}
				else
				{
					//MessageBox.Show(Info.ModelPath);
					string chemin = Info.ModelPath;

					Process.Start("C:\\TeklaStructures\\Macro\\SendPlanLaser.vbs", chemin);
	
				}
			}

            
        }
    }
}
