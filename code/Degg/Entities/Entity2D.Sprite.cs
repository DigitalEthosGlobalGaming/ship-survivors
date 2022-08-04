using Sandbox;

namespace Degg.Entities
{
	public partial class Entity2D
	{

		public bool PhysicsEnabled { get; set; }
		[Net]
		public SpriteSheetResource Sprite { get; set; }
		[Net]
		public string SpriteCode { get; set; }

		public void SetSprite( string code )
		{
			SpriteCode = code;
			SetMaterialGroup( code );
			SetupPhysics();
		}

		public string GetSprite()
		{
			return SpriteCode;
		}

		public Sprite? GetSpritesheetSprite()
		{
			if (Sprite?.Sprites.Count == 0)
			{
				return null;
			}

			return Sprite?.Sprites?.Find( ( item ) =>
			 {
				 return item.Code?.Trim()?.ToLower() == SpriteCode?.Trim()?.ToLower(); 
			 }) ?? Sprite.Sprites?[0];
		}

		public void SetupPhysics()
		{
			PhysicsEnabled = true;
			var nullableSprite = GetSpritesheetSprite();
			if ( nullableSprite.HasValue)
			{
				var sprite = nullableSprite.Value;

				if ( sprite.Shape == SpriteShapes.Square )
				{
					SetShape( sprite.Width, sprite.Height, Scale );
				} else
				{
					var largest = sprite.Width;
					if ( sprite.Height > largest)
					{
						largest = sprite.Height;
					}
					SetShape( largest, Scale );
				}
			}
		}

		public void SetSpritesheet( string path )
		{
			if (IsClient)
			{
				return;
			}
			var sprite = ResourceLibrary.Get<SpriteSheetResource>( path );
			Sprite = sprite;
			if ( sprite != null )
			{
				SetModel( sprite.Model );
				if ( PhysicsEnabled )
				{
					SetupPhysics();
				}
			}
			else
			{
				Log.Warning( "No sprite found for " + path );
			}
		}
	}
}
