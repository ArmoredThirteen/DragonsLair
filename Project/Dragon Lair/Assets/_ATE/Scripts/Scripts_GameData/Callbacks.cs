﻿

namespace Ate
{


	public delegate void Callback          ();
	public delegate void Callback<T>       (T arg1);
	public delegate void Callback<T, U>    (T arg1, U arg2);
	public delegate void Callback<T, U, V> (T arg1, U arg2, V arg3);

	public delegate Return ReturnCallback<Return>          ();
	public delegate Return ReturnCallback<Return, T>       (T arg1);
	public delegate Return ReturnCallback<Return, T, U>    (T arg1, U arg2);
	public delegate Return ReturnCallback<Return, T, U, V> (T arg1, U arg2, V arg3);


}//End Namespace
