import Content from "./Content";

export type EditUserType = {
  name: string;
};

type Props = {
  params: Promise<{ categoryId: string }>;
};
export default async function AdminAddOrEditCategoryPage({ params }: Props) {
  const { categoryId } = await params;

  return <Content categoryId={categoryId} />;
}
