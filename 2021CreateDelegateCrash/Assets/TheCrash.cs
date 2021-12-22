//#define DO_THE_METHOD
//#define DO_THE_LONG_METHOD
//#define DO_THE_SHORT_METHOD
//#define DO_THE_RET_METHOD
//#define DO_THE_OBJECTS_METHOD
//#define DO_THE_RET_OBJECTS_METHOD
#define DO_THE_RET_OBJECT_METHOD
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace some.ns
{
	public enum TheEnum { FOO, BAR }
	public partial class TheClass
	{
		#if DO_THE_METHOD
		// NOTE(theGiallo): this doesn't work
		public static void the_method( TheEnum e, int p )
		{
			Debug.Log( $"{e} {p}" );
		}
		#endif

		#if DO_THE_SHORT_METHOD
		// NOTE(theGiallo): this doesn't work
		public static void the_short_method( TheEnum e )
		{
			Debug.Log( $"{e}" );
		}
		#endif

		#if DO_THE_RET_METHOD
		// NOTE(theGiallo): this doesn't work
		public static TheEnum the_ret_method( int i )
		{
			Debug.Log( $"{i}" );
			return (TheEnum)i;
		}
		#endif

		#if DO_THE_LONG_METHOD
		// NOTE(theGiallo): this doesn't work
		public static void the_long_method( TheEnum e, int p, int p2, int p3, int p4, int p5 )
		{
			Debug.Log( $"{e} {p} {p2} {p3} {p4} {p5}" );
		}
		#endif

		#if DO_THE_OBJECTS_METHOD
		// NOTE(theGiallo): this works
		public static void the_objects_method(params object[] pars)
		{
			TheEnum e  = (TheEnum)pars[0];
			int     p  = (int    )pars[1];
			int     p2 = (int    )pars[2];
			int     p3 = (int    )pars[3];
			int     p4 = (int    )pars[4];
			int     p5 = (int    )pars[5];

			Debug.Log( $"{e} {p} {p2} {p3} {p4} {p5}" );
		}
		#endif

		#if DO_THE_RET_OBJECT_METHOD
		// NOTE(theGiallo): this doesn't work
		public static TheEnum the_ret_object_method(object p)
		{
			TheEnum e  = (TheEnum)p;
			Debug.Log( $"{e} {p}" );
			return e;
		}
		#endif


		#if DO_THE_RET_OBJECTS_METHOD
		// NOTE(theGiallo): this doesn't work
		public static TheEnum the_ret_objects_method(params object[] pars)
		{
			TheEnum e  = (TheEnum)pars[0];
			int     p  = (int    )pars[1];
			int     p2 = (int    )pars[2];
			int     p3 = (int    )pars[3];
			int     p4 = (int    )pars[4];
			int     p5 = (int    )pars[5];

			Debug.Log( $"{e} {p} {p2} {p3} {p4} {p5}" );
			return e;
		}
		#endif

	}
	public class TheCrash : MonoBehaviour
	{
		void Start()
		{
			the_crash();
		}
		private void the_crash()
		{
			Type t = typeof( TheClass );
			for ( int i = 0; i != 1; ++i )
			{
				log( $"doint iteration {i:D4}" );
				the_doing( t );
			}
		}
		private void the_doing( Type t )
		{
			log( $"the_doing( {t.Name} )" );
			var methodInfos = t.GetMethods( BindingFlags.Public | BindingFlags.Static );
			log( $"the_doing( {t.Name} ) methodInfos {methodInfos.Length}" );
			foreach ( var methodInfo in methodInfos )
			{
				log( $"the_doing( {t.Name} ) {methodInfo?.Name}" );
				log( $"the_doing( {t.Name} ) {methodInfo?.Name} GetParameters types..." );
				var types = methodInfo.GetParameters().Select( x => x.ParameterType );
				log( $"the_doing( {t.Name} ) {methodInfo?.Name} Append..." );
				types = types.Append( methodInfo.ReturnType );
				{
					int i = 0;
					foreach ( Type types_t in types )
					{
						log( $"the_doing( {t.Name} ) {methodInfo?.Name} types[{i}] = {types_t}" );
						++i;
					}
				}
				log( $"the_doing( {t.Name} ) {methodInfo?.Name} GetDelegateType({types?.Count()})..." );
				Type delegate_type = System.Linq.Expressions.Expression.GetDelegateType( types.ToArray() );
				log( $"the_doing( {t.Name} ) {methodInfo?.Name} CreateDelegate( {delegate_type.FullName} )..." );
				var deleg = methodInfo.CreateDelegate( delegate_type );
				log( $"the_doing( {t.Name} ) {methodInfo?.Name} SetValue..." );
				add_func(deleg);
			}

			log( $"the_doing( {t.Name} ) half" );

			var propInfos = t.GetProperties(BindingFlags.Public | BindingFlags.Static);
			log( $"the_doing( {t.Name} ) propInfos {propInfos.Length}" );
			foreach (var propInfo in propInfos)
			{
				log( $"the_doing( {t.Name} ) {propInfo?.Name}" );
				var getMethod = propInfo.GetGetMethod();
				var deleg = getMethod.CreateDelegate(System.Linq.Expressions.Expression.GetDelegateType(getMethod.ReturnType));
				add_func( deleg );
			}
			log( $"the_doing( {t.Name} ) end" );
		}
		private void add_func( Delegate deleg )
		{
			//delegates.Add( deleg );
		}
		private List<Delegate> delegates = new List<Delegate>();
		private void log( string msg ) => Debug.Log( msg );
	}
}
