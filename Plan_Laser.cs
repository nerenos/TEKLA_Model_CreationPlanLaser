using System.IO;
using System.Windows.Forms;
using System.Collections;

using Tekla.Structures;
using TSM = Tekla.Structures.Model;
using TSD = Tekla.Structures.Drawing;
using Tekla.Structures.Model.Operations;
using Tekla.Structures.Geometry3d;


#pragma warning disable 1633 // Unrecognized #pragma directive
#pragma reference "Tekla.Macros.Akit"
#pragma reference "Tekla.Macros.Runtime"
#pragma warning restore 1633 // Unrecognized #pragma directive

namespace Tekla.Technology.Akit.UserScript {
    public sealed class Script {

        public static void Run(Tekla.Technology.Akit.IScript akit) {
			
			Operation.DisplayPrompt("Création des plans pour DXF En Cour .");
			TSM.Model Model = new TSM.Model();
			
			
			CoordinateSystem AttribeView = new CoordinateSystem(new Point(), new Vector(1, 0, 0), new Vector(0, 0, -1));

			TSD.DrawingHandler DrawingHandler = new TSD.DrawingHandler();            
            if (DrawingHandler.GetConnectionStatus())
            {
				TSD.DrawingHandler  myDrwHndl = new TSD.DrawingHandler();
                TSD.DrawingEnumerator Croquis = DrawingHandler.GetDrawingSelector().GetSelected();
                Croquis.SelectInstances = false;
                if (Croquis.GetSize() == 0)
                {
                    Operation.DisplayPrompt("Aucun dessin sélectionné");
                    return;
                }
                while (Croquis.MoveNext())
                {	
					Operation.DisplayPrompt("Création des plans pour DXF En Cour .");
                    if ((Croquis.Current.GetType() == typeof(TSD.AssemblyDrawing)) && (Croquis != null))
                    {
                        TSD.AssemblyDrawing CroquisAcourant = Croquis.Current as TSD.AssemblyDrawing;
						if (CroquisAcourant != null)
                        {
                            TSM.Assembly assembly = Model.SelectModelObject(CroquisAcourant.AssemblyIdentifier) as TSM.Assembly;
                            if (assembly != null)
                            {
								TSD.Drawing DrawingCloned = new TSD.AssemblyDrawing(CroquisAcourant.AssemblyIdentifier, 1, "DWG");

								DrawingCloned.Insert();
								Operation.DisplayPrompt("Création des plans pour DXF En Cour ...");									
									TSD.DrawingObjectEnumerator AllAObjects = Croquis.Current.GetSheet().GetAllViews();
									
									while(AllAObjects.MoveNext())
								    {
									   if(AllAObjects.Current is TSD.View)
									   {
										   TSD.View currentview = AllAObjects.Current as TSD.View;
										   if(currentview.Name == "Vue de Face")
										   {
												AttribeView = currentview.DisplayCoordinateSystem;
										   }
									   }
									}

								
									TSD.DrawingObjectEnumerator AllObjects = DrawingCloned.GetSheet().GetAllViews();
									while(AllObjects.MoveNext())
								    {
									   if(AllObjects.Current is TSD.View)
									   {
										    TSD.View currentview = AllObjects.Current as TSD.View;
											currentview.DisplayCoordinateSystem = AttribeView;
											currentview.Modify();
									   }
									}
								myDrwHndl.SetActiveDrawing(DrawingCloned, false);
								DrawingCloned.PlaceViews();	
								myDrwHndl.CloseActiveDrawing(true);
									
								
                            }
                        }
						
                    }

                }
				Operation.DisplayPrompt("Terminé.....");

            }
        }
        
    }
}					
