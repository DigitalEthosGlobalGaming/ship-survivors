using Degg.Entities;
using Sandbox;

namespace ShipSurvivors
{
	public partial class Debris : Entity2D
	{
		
		public override void Spawn()
		{
			base.Spawn();
			SetShape(Entity2DShapes.Circle);
			
			EntityMaterial = "materials/bullets/bullet_1.vmat";
		}


	}
}
