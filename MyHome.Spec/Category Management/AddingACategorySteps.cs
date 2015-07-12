﻿using System;
using System.Configuration;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyHome.DataRepository;
using MyHome.Persistence;
using MyHome.Services;
using TechTalk.SpecFlow;

namespace MyHome.Spec
{
    [Binding]
    [Scope(Feature = "AddingACategory")]
    public class AddingACategorySteps
    {
        ICategoryService _categoryService;
        string _categoryName;

        private AccountingDataContext _dataContext;

        [BeforeScenario]
        public void Setup()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Database"];
            _dataContext = new AccountingDataContext(connectionString.ConnectionString);
            _dataContext.Database.BeginTransaction();

            _categoryService = null;
            _categoryName = "";
        }

        [AfterScenario]
        public void TearDown()
        {
            _dataContext.Database.CurrentTransaction.Rollback();
            _dataContext.Dispose();
        }

        #region Given

        [Given(@"The category type is '(.*)'")]
        public void GivenTheCategoryTypeIs(string categoryType)
        {
            switch (categoryType)
            {
                case "expense":
                    _categoryService = new ExpenseCategoryService(new ExpenseCategoryRepository(_dataContext));
                    break;
                case "income":
                    _categoryService = new IncomeCategoryService(new IncomeCategoryRepository(_dataContext));
                    break;
                case "paymentmethod":
                    _categoryService = new PaymentMethodService(new PaymentMethodRepository(_dataContext));
                    break;
            }
        }

        [Given(@"I have entered '(.*)' as the name")]
        public void GivenIHaveEnteredAsTheName(string name)
        {
            _categoryName = name;
        }

        [Given(@"there is no other category with that name")]
        public void GivenThereIsNoOtherCategoryWithThatName()
        {
            if (_categoryService.Exists(_categoryName))
            {
                _categoryService.Remove(_categoryName);
            }
        }

        [Given(@"there is another category with the same name")]
        public void GivenThereIsAnotherCategoryWithTheSameName()
        {
            try
            {
                _categoryService.Add(_categoryName);
            }
            catch (Exception e)
            {
                ScenarioContext.Current.Add("add_category_result", e);
            }
        }

        [Given(@"I have entered nothing for the name")]
        public void GivenIHaveEnteredNothingForTheName()
        {
            try
            {
                _categoryService.Add(_categoryName);
            }
            catch(ArgumentException e)
            {
                ScenarioContext.Current.Add("add_category_result", e);    
            }
        }

        #endregion

        #region When

        [When(@"I press add")]
        public void WhenIPressAdd()
        {
            try
            {
                _categoryService.Add(_categoryName);
            }
            catch (Exception e)
            {
                ScenarioContext.Current.Add("add_category_result", e);
            }
        }

        #endregion

        #region Then

        [Then(@"the handler returns an error indicator")]
        public void TheHandlerReturnsAnErrorIndicator()
        {
            var exception = ScenarioContext.Current.Get<Exception>("add_category_result");
            Assert.IsNotNull(exception);
        }

        [Then(@"the category should be added to the list")]
        public void ThenTheCategoryShouldBeAddedToTheList()
        {
            var result = _categoryService.GetAll().FirstOrDefault(c => string.Equals(c.Name, _categoryName));
            Assert.IsNotNull(result);
        }

        #endregion
    }
}