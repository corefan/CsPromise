﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Text;

using CsPromise;

namespace CsPromise.Test
{
    /// <summary>
    ///이 클래스는 PromiseTest에 대한 테스트 클래스로서
    ///PromiseTest 단위 테스트를 모두 포함합니다.
    ///</summary>
    [TestClass()]
    public class Promise212Test : BasePromiseTest {
        private TestContext testContextInstance;

        /// <summary>
        ///현재 테스트 실행에 대한 정보 및 기능을
        ///제공하는 테스트 컨텍스트를 가져오거나 설정합니다.
        ///</summary>
        public TestContext TestContext {
            get {
                return testContextInstance;
            }
            set {
                testContextInstance = value;
            }
        }

        #region 추가 테스트 특성
        // 
        //테스트를 작성할 때 다음 추가 특성을 사용할 수 있습니다.
        //
        //ClassInitialize를 사용하여 클래스의 첫 번째 테스트를 실행하기 전에 코드를 실행합니다.
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //ClassCleanup을 사용하여 클래스의 테스트를 모두 실행한 후에 코드를 실행합니다.
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //TestInitialize를 사용하여 각 테스트를 실행하기 전에 코드를 실행합니다.
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //TestCleanup을 사용하여 각 테스트를 실행한 후에 코드를 실행합니다.
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        [TestMethod()]
        public void PromiseMustNotTransitionToAnyOtherStateTest() {
            TestFulfilled(dummy_, (promise) => {
                var onFulfilledCalled = false;

                promise.Then((result) => {
                    onFulfilledCalled = true;
                }, (reason) => {
                    Assert.AreEqual(false, onFulfilledCalled);
                });

                SetTimeout(() => Assert.AreEqual(true, onFulfilledCalled), 100);
            });
        }

        [TestMethod()]
        public void PromiseMustNotTransitionTryingToFulfillThenImmediatelyReject() {
            var d = Deferred();
            var onFulfilledCalled = false;

            d.Promise.Then((result) => {
                onFulfilledCalled = true;
            }, (reason) => {
                Assert.AreEqual(false, onFulfilledCalled);
            });

            d.Resolve(dummy_);
            d.Reject(null);

            SetTimeout(() => Assert.AreEqual(true, onFulfilledCalled), 100);
        }

        [TestMethod()]
        public void PromiseMustNotTransitionTryingToFulfillThenRejectDelayed() {
            var d = Deferred();

            var onFulfilledCalled = false;

            d.Promise.Then((result) => {
                onFulfilledCalled = true;
            }, (reason) => {
                Assert.AreEqual(false, onFulfilledCalled);
            });

            SetTimeout(() => {
                d.Resolve(dummy_);
                d.Reject(null);
            }, 50);

            SetTimeout(() => Assert.AreEqual(true, onFulfilledCalled), 100);
        }

        [TestMethod()]
        public void PromiseMustNotTransitionTryingToFulfillImmediatelyThenRejectDelayed() {
            var d = Deferred();

            var onFulfilledCalled = false;

            d.Promise.Then((result) => {
                onFulfilledCalled = true;
            }, (reason) => {
                Assert.AreEqual(false, onFulfilledCalled);
            });

            d.Resolve(dummy_);
            SetTimeout(() => d.Reject(null), 50);
            SetTimeout(() => Assert.AreEqual(true, onFulfilledCalled), 100);
        }
    }
}

