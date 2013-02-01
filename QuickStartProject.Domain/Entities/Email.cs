using System;

namespace Logfox.Domain.Entities
{
	public class Email : IntIdDomainEntity
	{
		private string _to;
		private string _from;
		private string _subject;
		private string _cc;
		private string _body;
		private bool _isHtml;
		private bool _isSent;
		private int _sendAttempt;
		private bool _isForceSend;
		private EmailType _type;
		private DateTime _submitTime;
		private DateTime? _sendTime;

		protected Email() { }

		public Email(string to, string from, string cc, string subject, string body, bool isHtml, EmailType type)
		{
			_to = to;
			_from = from;
			_cc = cc;
			_subject = subject;
			_body = body;
			_isHtml = isHtml;
			_isForceSend = false;
			_sendAttempt = 0;
			_type = type;
			_isSent = false;
			_submitTime = DateTime.UtcNow;
			_sendTime = null;
		}

		public virtual string To
		{
			get { return _to; }
			protected set { _to = value; }
		} 

		public virtual string From
		{
			get { return _from; }
			protected set { _from = value; }
		}

		public virtual string Subject
		{
			get { return _subject; }
			protected set { _subject = value; }
		}

		public virtual string Cc
		{
			get { return _cc; }
			protected set { _cc = value; }
		}

		public virtual string Body
		{
			get { return _body; }
			protected set { _body = value; }
		}

		public virtual bool IsHtml
		{
			get { return _isHtml; }
			protected set { _isHtml = value; }
		}

		public virtual bool IsSent
		{
			get { return _isSent; }
			set { _isSent = value; }
		}

		public virtual int SendAttempt
		{
			get { return _sendAttempt; }
			set { _sendAttempt = value; }
		}

		/// <summary>
		/// Is used to forse resend email if required
		/// </summary>
		public virtual bool IsForceSend
		{
			get { return _isForceSend; }
			set { _isForceSend = value; }
		}

		public virtual EmailType Type
		{
			get { return _type; }
			protected set { _type = value; }
		}

		public virtual DateTime SubmitTime
		{
			get { return _submitTime; }
			protected set { _submitTime = value; }
		}

		public virtual DateTime? SendTime
		{
			get { return _sendTime; }
			set { _sendTime = value; }
		}
	}
}
