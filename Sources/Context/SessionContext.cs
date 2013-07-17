/*
 * SharpDevelop으로 작성되었습니다.
 * 사용자: spowner
 * 날짜: 2013-07-17
 * 시간: 오전 11:33
 * 
 * 이 템플리트를 변경하려면 [도구->옵션->코드 작성->표준 헤더 편집]을 이용하십시오.
 */
using System;
using DSW.Core.Context;
using Nancy.Security;

namespace DSW.Context
{
	/// <summary>
	/// Description of SessionContext.
	/// </summary>
	public class SessionContext : ISessionContext
	{
		public SessionContext()
		{
		}
		
		public IUserIdentity GetUserFromIdentifier(Guid identifier, Nancy.NancyContext context)
		{
			return null;
		}
	}
}
