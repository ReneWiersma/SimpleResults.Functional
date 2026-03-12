namespace SoftwareMadeSimple.SimpleResults.Functional;

public static class LiftResult<E>
{
    public static Result<Func<T, R>, E> Lift<T, R>(Func<T, R> f) =>
        Result<Func<T, R>, E>.Success(f);

    public static Result<Func<T1, Func<T2, R>>, E> Lift<T1, T2, R>(Func<T1, T2, R> f) =>
        Result<Func<T1, Func<T2, R>>, E>.Success(t1 => t2 => f(t1, t2));

    public static Result<Func<T1, Func<T2, Func<T3, R>>>, E> Lift<T1, T2, T3, R>(Func<T1, T2, T3, R> f) =>
        Result<Func<T1, Func<T2, Func<T3, R>>>, E>.Success(t1 => t2 => t3 => f(t1, t2, t3));

    public static Result<Func<T1, Func<T2, Func<T3, Func<T4, R>>>>, E> Lift<T1, T2, T3, T4, R>(Func<T1, T2, T3, T4, R> f) =>
        Result<Func<T1, Func<T2, Func<T3, Func<T4, R>>>>, E>.Success(t1 => t2 => t3 => t4 => f(t1, t2, t3, t4));

    public static Result<Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, R>>>>>, E> Lift<T1, T2, T3, T4, T5, R>(Func<T1, T2, T3, T4, T5, R> f) =>
        Result<Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, R>>>>>, E>.Success(t1 => t2 => t3 => t4 => t5 => f(t1, t2, t3, t4, t5));

    public static Result<Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, R>>>>>>, E> Lift<T1, T2, T3, T4, T5, T6, R>(Func<T1, T2, T3, T4, T5, T6, R> f) =>
        Result<Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, R>>>>>>, E>.Success(t1 => t2 => t3 => t4 => t5 => t6 => f(t1, t2, t3, t4, t5, t6));

    public static Result<Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, R>>>>>>>, E> Lift<T1, T2, T3, T4, T5, T6, T7, R>(Func<T1, T2, T3, T4, T5, T6, T7, R> f) =>
        Result<Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, R>>>>>>>, E>.Success(t1 => t2 => t3 => t4 => t5 => t6 => t7 => f(t1, t2, t3, t4, t5, t6, t7));

    public static Result<Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, R>>>>>>>>, E> Lift<T1, T2, T3, T4, T5, T6, T7, T8, R>(Func<T1, T2, T3, T4, T5, T6, T7, T8, R> f) =>
        Result<Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, R>>>>>>>>, E>.Success(t1 => t2 => t3 => t4 => t5 => t6 => t7 => t8 => f(t1, t2, t3, t4, t5, t6, t7, t8));

    public static Result<Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, R>>>>>>>>>, E> Lift<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R> f) =>
        Result<Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, R>>>>>>>>>, E>.Success(t1 => t2 => t3 => t4 => t5 => t6 => t7 => t8 => t9 => f(t1, t2, t3, t4, t5, t6, t7, t8, t9));

    public static Result<Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, R>>>>>>>>>>, E> Lift<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R> f) =>
        Result<Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, R>>>>>>>>>>, E>.Success(t1 => t2 => t3 => t4 => t5 => t6 => t7 => t8 => t9 => t10 => f(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10));

    public static Result<Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, R>>>>>>>>>>>, E> Lift<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R> f) =>
        Result<Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, R>>>>>>>>>>>, E>.Success(t1 => t2 => t3 => t4 => t5 => t6 => t7 => t8 => t9 => t10 => t11 => f(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11));

    public static Result<Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, R>>>>>>>>>>>>, E> Lift<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R> f) =>
        Result<Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, R>>>>>>>>>>>>, E>.Success(t1 => t2 => t3 => t4 => t5 => t6 => t7 => t8 => t9 => t10 => t11 => t12 => f(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12));

    public static Result<Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, R>>>>>>>>>>>>>, E> Lift<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R> f) =>
        Result<Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, R>>>>>>>>>>>>>, E>.Success(t1 => t2 => t3 => t4 => t5 => t6 => t7 => t8 => t9 => t10 => t11 => t12 => t13 => f(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13));

    public static Result<Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, R>>>>>>>>>>>>>>, E> Lift<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R> f) =>
        Result<Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, R>>>>>>>>>>>>>>, E>.Success(t1 => t2 => t3 => t4 => t5 => t6 => t7 => t8 => t9 => t10 => t11 => t12 => t13 => t14 => f(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14));

    public static Result<Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, Func<T15, R>>>>>>>>>>>>>>>, E> Lift<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R> f) =>
        Result<Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, Func<T15, R>>>>>>>>>>>>>>>, E>.Success(t1 => t2 => t3 => t4 => t5 => t6 => t7 => t8 => t9 => t10 => t11 => t12 => t13 => t14 => t15 => f(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15));

    public static Result<Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, Func<T15, Func<T16, R>>>>>>>>>>>>>>>>, E> Lift<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R> f) =>
        Result<Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, Func<T15, Func<T16, R>>>>>>>>>>>>>>>>, E>.Success(t1 => t2 => t3 => t4 => t5 => t6 => t7 => t8 => t9 => t10 => t11 => t12 => t13 => t14 => t15 => t16 => f(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16));
}