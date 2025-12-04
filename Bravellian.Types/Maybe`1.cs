// Copyright (c) Samuel McAravey
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Bravellian;

public readonly record struct Maybe<T>
{
    public static readonly Maybe<T> None = new();
    private readonly T value = default!;

    public Maybe()
    {
        this.HasValue = false;
    }

    public Maybe(T value)
    {
        this.HasValue = true;
        this.value = value;
    }

    public Maybe(T value, bool nullIsNone)
    {
        if (nullIsNone && value is null)
        {
            this.HasValue = false;
        }
        else
        {
            this.HasValue = true;
            this.value = value;
        }
    }

    public bool HasValue { get; }

    public T Value
    {
        get
        {
            return !this.HasValue ? throw new InvalidOperationException("No value.") : this.value;
        }
    }

    public static implicit operator Maybe<T>(T value) => new(value);

    public static implicit operator Maybe<object>(Maybe<T> value)
    {
        return !value.HasValue ? Maybe<object>.None : new Maybe<object>(value.Value);
    }

    public static implicit operator Maybe<object>(Maybe<T>? value)
    {
        return value is null ? Maybe<object>.None : new Maybe<object>(value.Value);
    }

    public TResult Match<TResult>(Func<TResult> none, Func<T, TResult> some)
    {
        return this.HasValue ? some(this.Value) : none();
    }

    public Maybe<TResult> Select<TResult>(Func<T, TResult> f)
    {
        return this.HasValue ? new Maybe<TResult>(f(this.Value)) : Maybe<TResult>.None;
    }

    public Maybe<TResult> SelectMany<TResult>(Func<T, Maybe<TResult>> f)
    {
        return this.HasValue ? f(this.Value) : Maybe<TResult>.None;
    }

    public Maybe<TResult> SelectMany<TCollection, TResult>(Func<T, Maybe<TCollection>> collectionSelector, Func<T, TCollection, TResult> resultSelector)
    {
        if (this.HasValue)
        {
            T val = this.Value;
            Maybe<TCollection> mTColl = collectionSelector(val);
            Maybe<TResult> result = mTColl.Select(tColl => resultSelector(val, tColl));
            return result;
        }

        return Maybe<TResult>.None;
    }

    public T Or(T value)
    {
        if (this.HasValue)
        {
            return this.Value;
        }

        return value;
    }

    public T GetValueOrDefault(T defaultValue)
    {
        return this.HasValue ? this.Value : defaultValue;
    }

    public T GetValueOrDefault()
    {
        return this.HasValue ? this.Value : default;
    }

    public bool TryGetValue([MaybeNullWhen(false)] out T value)
    {
        if (this.HasValue)
        {
            value = this.Value;
            return true;
        }

        value = default;
        return false;
    }
}
