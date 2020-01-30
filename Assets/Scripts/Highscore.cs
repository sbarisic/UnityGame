using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HighscoreItem : IValueSerializable {
	public string Name;
	public int Points;

	public HighscoreItem() {
	}

	public HighscoreItem(string Name, int Points) {
		this.Name = Name;
		this.Points = Points;
	}

	public void Deserialize(BinaryReader BR) {
		Name = BR.ReadString();
		Points = BR.ReadInt32();
	}

	public void Serialize(BinaryWriter BW) {
		BW.Write(Name);
		BW.Write(Points);
	}
}

public class Highscore : IValueSerializable {
	List<HighscoreItem> Items = new List<HighscoreItem>();

	public void Deserialize(BinaryReader BR) {
		Items.Clear();
		int Count = BR.ReadInt32();

		for (int i = 0; i < Count; i++) {
			HighscoreItem Itm = new HighscoreItem();
			Itm.Deserialize(BR);
			Items.Add(Itm);
		}

		SortList();
	}

	public void Serialize(BinaryWriter BW) {
		BW.Write(Items.Count);

		for (int i = 0; i < Items.Count; i++)
			Items[i].Serialize(BW);
	}

	void SortList() {
		Items.Sort((A, B) => B.Points.CompareTo(A.Points));
	}

	public void Add(HighscoreItem Itm) {
		Items.Add(Itm);
		SortList();

		if (Items.Count > 10)
			Items = new List<HighscoreItem>(Items.Take(10));

		ValueSerializer.SetValue(nameof(Highscore), this);
	}

	public void Add(string Name, int Points) {
		Add(new HighscoreItem(Name, Points));
	}

	public override string ToString() {
		if (Items.Count == 0)
			return "List Empty";

		StringBuilder SB = new StringBuilder();

		for (int i = 0; i < Items.Count; i++)
			SB.AppendLine(string.Format("{0} - {1}", Items[i].Points, Items[i].Name));

		return SB.ToString();
	}

	// Singleton stuff
	static Highscore List;

	public static Highscore GetInstance() {
		if (List == null) {
			List = ValueSerializer.GetValue<Highscore>(nameof(Highscore));

			if (List == null)
				List = new Highscore();
		}

		return List;
	}
}