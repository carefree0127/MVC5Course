using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using MVC5Course.Models.ViewModel;

namespace MVC5Course.Models
{   
	public  class ProductRepository : EFRepository<Product>, IProductRepository
	{
        //�[�J�@�����z����
        public override IQueryable<Product> All()
        {
            return base.All().Where(p=>!p.is�R��);
        }

        //�P�_�O�_�n�ϥ���ܩҦ����
        public IQueryable<Product> All(bool ShowAll) {
            if (ShowAll)
            {
                return base.All();
            }
            else {
                return this.All();
            }
        }

        //API�X�R
        public Product get�浧���ByProductID(int id) {
            return this.All().FirstOrDefault(p => p.ProductId == id);
        }

        public IQueryable<Product> getProduct�C���Ҧ����(bool Active, bool ShowAll=false)
        {
            IQueryable<Product> all = this.All();//�w�]ShowAll = false
            if (ShowAll) { // �p�G���nShowAll���ݨD
                all = base.All();
            }
            /*
             .Where(p => p.Active.HasValue && p.Active.Value == Active)
                .OrderByDescending(p => p.ProductId).Take(10)
             */
            return all
                .Where(p => p.Active.Value == Active)
                .OrderByDescending(p => p.ProductId).Take(10); 
        }

        public void Update(Product product) {
            //db.Entry(product).State = EntityState.Modified;
            this.UnitOfWork.Context.Entry(product).State = EntityState.Modified;
        }

        public override void Delete(Product entity)
        {
            //��������
            //��ĳ�g�bController
            //this.UnitOfWork.Context.Configuration.ValidateOnSaveEnabled = false;
            entity.is�R�� = true;
            //this.UnitOfWork.Commit(); //���Controller�B�z
        }
    }

	public  interface IProductRepository : IRepository<Product>
	{

	}
}