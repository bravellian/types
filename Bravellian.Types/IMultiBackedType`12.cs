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

#pragma warning disable S2436 // Types and methods should not have too many generic parameters
public interface IMultiBackedType<TSelf, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : IParsable<TSelf>
#pragma warning restore S2436 // Types and methods should not have too many generic parameters
    where TSelf : IMultiBackedType<TSelf, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
    where T1 : IParsable<T1>, IComparable<T1>
    where T2 : IParsable<T2>, IComparable<T2>
    where T3 : IParsable<T3>, IComparable<T3>
    where T4 : IParsable<T4>, IComparable<T4>
    where T5 : IParsable<T5>, IComparable<T5>
    where T6 : IParsable<T6>, IComparable<T6>
    where T7 : IParsable<T7>, IComparable<T7>
    where T8 : IParsable<T8>, IComparable<T8>
    where T9 : IParsable<T9>, IComparable<T9>
    where T10 : IParsable<T10>, IComparable<T10>
    where T11 : IParsable<T11>, IComparable<T11>;
