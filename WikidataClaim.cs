namespace WikiAccess {

	using System;

	public enum ClaimType {

		Null,

		String,

		Int,

		DateTime
	}

	/// <summary>
	///     Container to hold a Wikidata claim.
	/// </summary>
	public class WikidataClaim {

		private Wikidate _valueAsDateTime;

		private Int32 _valueAsInt;

		private String _valueAsString;

		public Int32 Pcode { get; set; }

		public Int32 Qcode { get; set; }

		public ClaimType Type { private set; get; }

		public Wikidate ValueAsDateTime {
			get => this._valueAsDateTime;

			set {
				this._valueAsDateTime = value;
				this.Type = ClaimType.DateTime;
			}
		}

		public Int32 ValueAsInt {
			get => this._valueAsInt;

			set {
				this._valueAsInt = value;
				this.Type = ClaimType.Int;
			}
		}

		public String ValueAsString {
			get => this._valueAsString;

			set {
				this._valueAsString = value;
				this.Type = ClaimType.String;
			}
		}

		public WikidataClaim() {
			this._valueAsDateTime = new Wikidate();
			this.Type = new ClaimType();
			this.Qcode = 0;
		}

		public override String ToString() {
			switch ( this.Type ) {
				case ClaimType.DateTime: return this.ValueAsDateTime.ToString();

				case ClaimType.Int: return this.ValueAsInt.ToString();

				default: return this.ValueAsString;
			}
		}
	}
}