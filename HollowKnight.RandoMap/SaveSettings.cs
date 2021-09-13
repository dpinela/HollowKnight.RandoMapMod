using SereCore;

namespace RandoMapMod {
	public class SaveSettings : BaseSettings {
		#region Statics
		public static SaveSettings Instance;
		#endregion

		#region Constructors
		public SaveSettings() {
			AfterDeserialize += () => {
				//This space probably unintentially left blank
			};
			Instance = this;
		}
		#endregion

		#region Non-Private Non-Methods
		public bool MapsGiven {
			get => GetBool(false);
			set => SetBool(value);
		}

		public bool RandoPoolOn {
			get => GetBool(false);
			set => SetBool(value);
		}

		public bool UnknownOn {
			get => GetBool(false);
			set => SetBool(value);
		}

		public bool Group1On {
			get => GetBool(true);
			set => SetBool(value);
		}

		public bool Group2On {
			get => GetBool(true);
			set => SetBool(value);
		}

		public bool Group3On {
			get => GetBool(true);
			set => SetBool(value);
		}

		public bool Group4On {
			get => GetBool(true);
			set => SetBool(value);
		}

		public bool Group5On {
			get => GetBool(true);
			set => SetBool(value);
		}

		public bool Group6On {
			get => GetBool(true);
			set => SetBool(value);
		}
		#endregion
	}
}
