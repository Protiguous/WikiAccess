namespace WikiAccess {

	using System;

	public class ErrorMessage {

		public Int32 Code { get; set; }

		public String Message { get; set; }

		public String Module { get; set; }

		public String SystemMessage { get; set; }

		public ErrorMessage( String module, Int32 code, String message, String systemMessage = null ) {
			this.Module = module;
			this.Code = code;
			this.Message = message;
			this.SystemMessage = systemMessage;
		}

		public override String ToString() {
			var returnMessage = this.Module + this.Code.ToString( "000" ) + ": " + this.Message;

			if ( !String.IsNullOrWhiteSpace( this.SystemMessage ) ) {
				returnMessage += " (" + this.SystemMessage + ")";
			}

			return returnMessage;
		}
	}
}