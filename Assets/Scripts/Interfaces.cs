using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Scripts {
	enum DamageType {
		Default = 0,
		Fire
	}

	interface IGameObject {
	}

	interface IDamagable  {
		void DealDamage(int Damage, DamageType DType = DamageType.Default);
	}

	interface IAlive : IDamagable {
		int GetHealth();

		void Heal(int HP);
	}
}
