import { Category } from "./categories";
import apiSsr from "./core/apiSsr";
import urls from "./core/urls";

export type Product = {
  id: number;
  title: string;
  description: string;
  price: number;
  category: Category;
  image: string;
};

export const getProducts = async (): Promise<Product[]> => {
  const response = await apiSsr().get<Product[]>(urls.product.list);
  return response.data;
};
