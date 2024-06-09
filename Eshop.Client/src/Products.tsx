import { useEffect, useState } from "react";
import { CategoryType } from "./Categories";

type ProductType = {
  id: number;
  title: string;
  description: string;
  price: number;
  category: CategoryType;
};

type Props = {
  categoryId: number;
};
const Products = ({ categoryId }: Props) => {
  const [products, setProducts] = useState<ProductType[]>([]);

  useEffect(() => {
    const loadProducts = async () => {
      const productResult = await (
        await fetch("https://localhost:7203/Product")
      ).json();

      setProducts(
        productResult.filter(
          (x: { category: { id: number } }) => x.category.id === categoryId
        )
      );
    };

    loadProducts();
  }, [categoryId]);

  return (
    <div>
      {products.map((product) => {
        return (
          <h2 key={product.id}>
            {product.title} - {product.price} - {product.description}
          </h2>
        );
      })}
    </div>
  );
};

export default Products;
