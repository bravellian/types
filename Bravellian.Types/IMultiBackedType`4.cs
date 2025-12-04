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

public interface IMultiBackedType<TSelf, T1, T2, T3> : IParsable<TSelf>
    where TSelf : IMultiBackedType<TSelf, T1, T2, T3>
    where T1 : IParsable<T1>, IComparable<T1>
    where T2 : IParsable<T2>, IComparable<T2>
    where T3 : IParsable<T3>, IComparable<T3>;
