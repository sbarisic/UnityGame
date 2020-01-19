using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Scripts {
	public enum DamageType {
		Default = 0,
		Fire
	}

	public interface IGameObject {
	}

	public interface IDamagable {
		void DealDamage(int Damage, DamageType DType = DamageType.Default);
	}

	public interface IAlive : IDamagable {
		int GetHealth();

		void Heal(int HP);
	}
}
